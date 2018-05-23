using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetACardCommand : Command {

    private bool fast;
    private int ID;
    private bool fromDeck;
    private CardLogic cl;
    public GetACardCommand(CardLogic cl,  Player p, bool fast, bool fromDeck)
    {
        this.cl = cl;
        CommandSender = p;
        this.fast = fast;
        this.fromDeck = fromDeck;
    }
    public override void StartCommandExecution()
    {
        CommandSender.PArea.handVisual.GivePlayerACard(cl.ca, cl.UniqueCardID, fast, fromDeck);
        CommandSender.hand.CardsInHand.Add(cl);
        if (fromDeck)
            CommandSender.deck.cards.RemoveAt(0);
    }
}
