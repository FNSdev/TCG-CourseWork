using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//TODO
/*
 *       Корректный обмен информацией о character assets
 * DONE  тригерить ивенты вроде creature attack / another creature died / ... только для low player 
 *       fatigue damage
 *       card burn if hand is full
 * DONE  превью своих существ на столе?
 * DONE? мана увеличивается после 10?
 * BUG   Карты берутся, когда рука уже полная
 *       Проблемы, когда в руке 9 карт, умирает существо которое даёт монетку и на столе крича, дровающая карту
 *       при смерти другого существа
*/

public class PlayerConnection : NetworkBehaviour {

    static int id;
   // bool gameStarted = false;
    bool SecondPlayerConnected = false;

	void Start ()
    {
        if (!isLocalPlayer)
            return;

        NetworkManager.singleton.client.RegisterHandler(MessageType.Command, ClientRecievedCommand);
        NetworkServer.RegisterHandler(MessageType.Command, ServerRecievedCommand);

        NetworkManager.singleton.client.RegisterHandler(MessageType.DeckInfo, ClientRecievedDeckInfo);
        NetworkServer.RegisterHandler(MessageType.DeckInfo, ServerRecievedDeckInfo);

        if (isServer)
            id = 0;
        else
        {
            id = -1;
            SecondPlayerConnected = true;
        }
        Debug.Log("New player connected! Player ID = " + id);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(SecondPlayerConnected && isLocalPlayer)
        {
            Debug.Log("Two players online! Statring the game ... ");
            SecondPlayerConnected = false;
            TurnManager.Instance.Test();
            SendCommandOnServer(0, CommandType.Initialize);
        }
    }

    static public void SendCommandOnServer(byte Sender, CommandType CType, params string [] options)
    {
        CommandMessage CM = new CommandMessage();
        CM.Command = CType;
        CM.connectionID = (short)id;
        CM.options = options;
        CM.Player = Sender == 1 ? (byte)2 : (byte)1;

        Debug.Log("Sending command on the server ... ");
        NetworkManager.singleton.client.Send(MessageType.Command, CM);

    }

    private void ServerRecievedCommand(NetworkMessage netMsg)
    {
        CommandMessage CM = netMsg.ReadMessage<CommandMessage>();
        NetworkServer.SendToClient(CM.connectionID + 1, MessageType.Command, CM);

        Debug.Log("Server recieved command ("+CM.Command+")! Sending to: " + (CM.connectionID + 1));
    }

    private void ClientRecievedCommand(NetworkMessage netMsg)
    {
        if (!isLocalPlayer)
            return;

        Debug.Log("Client recieved command!");
        CommandMessage CM = netMsg.ReadMessage<CommandMessage>();
        if(CM.Command == CommandType.Initialize)
        {
            TurnManager.Instance.OnPVPGameStartTest();
            Debug.Log("!!!");
            return;
        }
        Player player; 
        if (CM.Player == 1)
            player = GlobalSettings.Instance.TopPlayer;
        else 
            player = GlobalSettings.Instance.LowPlayer;
        switch (CM.Command)
        {
            case CommandType.StartATurn:
                new StartATurnCommand(player).AddToQueue();
                break;
            case CommandType.DrawACard:
                bool fast = CM.options[0] == "1" ? true : false;
                new DrawACardCommand(player, fast : fast, fromDeck : true).AddToQueue();
                break;
            case CommandType.GetACard:
                CardLogic cl = new CardLogic(Collection.Instance.FindAsset(CM.options[0]));
                cl.owner = player;
                new GetACardCommand(cl, player, fast : true, fromDeck : false).AddToQueue();
                break;
            case CommandType.UpdateManaBar:
                new UpdateManaCrystalsCommand(player, System.Int32.Parse(CM.options[0]), System.Int32.Parse(CM.options[1])).AddToQueue();
                break;
            case CommandType.PlayACreature:
                CreatureLogic newCreature = new CreatureLogic(player, Collection.Instance.FindAsset(CM.options[2]));
                new PlayACreatureCommand(System.Int32.Parse(CM.options[0]), player, System.Int32.Parse(CM.options[1]), newCreature).AddToQueue();
                break;
            case CommandType.SummonACreature:
                CardAsset ca = Collection.Instance.FindAsset(CM.options[1]);
                CreatureLogic summonedCreature = new CreatureLogic(player, ca);
                new SummonACreatureCommand(player, System.Int32.Parse(CM.options[0]), summonedCreature).AddToQueue();
                break;
            case CommandType.AttackCreature:
                int TargetID = System.Int32.Parse(CM.options[0]);
                if (TargetID == 1)
                    TargetID = 2;
                else if (TargetID == 2)
                    TargetID = 1;
                int AttackerID = System.Int32.Parse(CM.options[1]);
                int TargetAttack = System.Int32.Parse(CM.options[2]);
                int AttackerAttack = System.Int32.Parse(CM.options[3]);
                int AttackerHealthAfter = System.Int32.Parse(CM.options[4]);
                int TargetHealthAfter = System.Int32.Parse(CM.options[5]);
                new CreatureAttackCommand(TargetID, AttackerID, TargetAttack, AttackerAttack, AttackerHealthAfter, TargetHealthAfter).AddToQueue();
                break;
            case CommandType.PlayASpell:
                new PlayASpellCardCommand(player, System.Int32.Parse(CM.options[0])).AddToQueue();
                break;
            case CommandType.UseHeroPower:
                HeroPowerButton button = player.PArea.GetComponentInChildren<HeroPowerButton>();
                button.WasUsedThisTurn = true;
                break;
            case CommandType.DealDamage:
                int targetID;
                if (CM.options[0] == "1")
                    targetID = 2;
                else if (CM.options[0] == "2")
                    targetID = 1;
                else
                    targetID = System.Int32.Parse(CM.options[0]);
                new DealDamageCommand(targetID, System.Int32.Parse(CM.options[1]), System.Int32.Parse(CM.options[2])).AddToQueue();
                break;
        }
    }

    public static void SendDeckInfoOnServer(byte Sender, string[] assets)
    {
        DeckInfoMessage DIM = new DeckInfoMessage();
        DIM.Assets = assets;
        DIM.connectionID = (short)id;
        DIM.Player = Sender == 1 ? (byte)2 : (byte)1;
        NetworkManager.singleton.client.Send(MessageType.DeckInfo, DIM);
    }

    private void ServerRecievedDeckInfo(NetworkMessage netMsg)
    {
        DeckInfoMessage DIM = netMsg.ReadMessage<DeckInfoMessage>();
        NetworkServer.SendToClient(DIM.connectionID + 1, MessageType.DeckInfo, DIM);
    }

    private void ClientRecievedDeckInfo(NetworkMessage netMsg)
    {
        //TODO
        /*
         * Работает не совсем адекватно
         * игрок-клиент(второй подключившийся) отправляет зашафленные колоды на своей стороне другому игроку
         * на стороне первого игрока(игрока-сервера) колоды копируются симметричным образом
         * нехорошо, когда будет выбор колод??
        */

        if (!isLocalPlayer)
            return;
        DeckInfoMessage DIM = netMsg.ReadMessage<DeckInfoMessage>();
        Player p = DIM.Player == 1 ? GlobalSettings.Instance.TopPlayer : GlobalSettings.Instance.LowPlayer;
        p.deck.cards.Clear();
        foreach(string asset in DIM.Assets)
        {
            CardAsset CA = Collection.Instance.FindAsset(asset);
            p.deck.cards.Add(CA);
        }
    }
}


