using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour, ICharacter
{
    public int PlayerID;
    public CharacterAsset charAsset;
    public PlayerArea PArea;
    public SpellEffect HeroPowerEffect;

    public Deck deck;
    public Hand hand;
    public Table table;

    public Transform StartingPosition;


    private int bonusManaThisTurn = 0;
    public bool usedHeroPowerThisTurn = false;
    public const int maxAmountOfMana = 10;

    public int ID
    {
        get{ return PlayerID; }
    }

    private int manaThisTurn;
    public int ManaThisTurn
    {
        get{ return manaThisTurn;}
        set
        {
            manaThisTurn = value <= 10 ? value : maxAmountOfMana;
            new UpdateManaCrystalsCommand(this, manaThisTurn, manaLeft).AddToQueue();
        }
    }

    private int manaLeft;
    public int ManaLeft
    {
        get
        { return manaLeft; }
        set
        {
            manaLeft = value;
            new UpdateManaCrystalsCommand(this, ManaThisTurn, manaLeft).AddToQueue();
            PlayerConnection.SendCommandOnServer((byte)ID, CommandType.UpdateManaBar, ManaThisTurn.ToString(), manaLeft.ToString());
            if (TurnManager.Instance.WhoseTurn == this)
                HighlightPlayableCards();
        }
    }

    public Player otherPlayer
    {
        get
        {
            if (Players[0] == this)
                return Players[1];
            else
                return Players[0];
        }
    }

    private int health;
    public int Health
    {
        get { return health;}
        set
        {
            health = value;
            if (value <= 0)
                Die(); 
        }
    }

    public delegate void VoidWithNoArguments();
    //public event VoidWithNoArguments CreaturePlayedEvent;
    //public event VoidWithNoArguments SpellPlayedEvent;
    //public event VoidWithNoArguments StartTurnEvent;
    public event VoidWithNoArguments EndTurnEvent;

    public static Player[] Players;

    void Awake()
    {
        Players = GameObject.FindObjectsOfType<Player>();
        PlayerID = IDFactory.GetUniqueID();
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            DrawACard();
        }
    }

    public virtual void OnTurnStart()
    {
        Debug.Log("In ONTURNSTART for "+ gameObject.name);
        usedHeroPowerThisTurn = false;
        ManaThisTurn++;
        ManaLeft = ManaThisTurn;
        foreach (CreatureLogic cl in table.CreaturesOnTable)
            cl.OnTurnStart();
        PArea.HeroPower.WasUsedThisTurn = false;

    }

    public void GetBonusMana(int amount)
    {
        bonusManaThisTurn += amount;
        ManaThisTurn += amount;
        ManaLeft += amount;
    }   

    public void OnTurnEnd()
    {
        if(EndTurnEvent != null)
            EndTurnEvent.Invoke();
        ManaThisTurn -= bonusManaThisTurn;
        bonusManaThisTurn = 0;
        GetComponent<TurnMaker>().StopAllCoroutines();
        TableVisual TVistual = PArea.tableVisual.GetComponent<TableVisual>();
        foreach(GameObject creature in TVistual.CreaturesOnTable)
        {
            //if (CreatureLogic.CreaturesCreatedThisGame[creature.GetComponent<IDHolder>().UniqueID].TargetedBattlecry == true) // будет ошибка, если существо умерло до боевого клича?
                creature.GetComponentInChildren<DragCreatureBattlecry>().TurnTargetingOff();
        }
        foreach (CreatureLogic cl in table.CreaturesOnTable)
            cl.OnTurnEnd();
    }

    public bool DrawACard(bool fast = false)
    {
        if (deck.cards.Count > 0)
        {
            if (hand.CardsInHand.Count < PArea.handVisual.slots.Children.Length)
            {
                new DrawACardCommand(this, fast, fromDeck: true).AddToQueue();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // there are no cards in the deck, take fatigue damage.
            return false;
        }
       
    }

    public void DrawACoin()
    {
        if (hand.CardsInHand.Count < PArea.handVisual.slots.Children.Length)
        {
            CardLogic newCard = new CardLogic(GlobalSettings.Instance.CoinCard);
            newCard.owner = this;
            new GetACardCommand(newCard, this, fast: true, fromDeck: false).AddToQueue();
        }
    }

    public void PlayASpellFromHand(int SpellCardUniqueID, int TargetUniqueID)
    {
        // if TargetUnique ID < 0 , for example = -1, there is no target.
        if (TargetUniqueID < 0)
            PlayASpellFromHand(CardLogic.CardsCreatedThisGame[SpellCardUniqueID], null);
        else if (TargetUniqueID == ID)
        {
            PlayASpellFromHand(CardLogic.CardsCreatedThisGame[SpellCardUniqueID], this);
        }
        else if (TargetUniqueID == otherPlayer.ID)
        {
            PlayASpellFromHand(CardLogic.CardsCreatedThisGame[SpellCardUniqueID], this.otherPlayer);
        }
        else
        {
            // target is a creature
            PlayASpellFromHand(CardLogic.CardsCreatedThisGame[SpellCardUniqueID], CreatureLogic.CreaturesCreatedThisGame[TargetUniqueID]);
        }
          
    }

    public void PlayASpellFromHand(CardLogic playedCard, ICharacter target)
    {
        if (playedCard.effect != null)
            playedCard.effect.ActivateEffect(playedCard.ca.specialSpellAmount, target);
        else
        {
            Debug.LogWarning("No effect found on card " + playedCard.ca.name);
        }
        new PlayASpellCardCommand(this, playedCard.ID).AddToQueue();
        PlayerConnection.SendCommandOnServer((byte)ID, CommandType.PlayASpell, playedCard.ID.ToString());
        ManaLeft -= playedCard.CurrentManaCost;
    }

    public void PlayACreatureFromHand(int UniqueID, int tablePos)
    {
        PlayACreatureFromHand(CardLogic.CardsCreatedThisGame[UniqueID], tablePos);
    }

    public void PlayACreatureFromHand(CardLogic playedCard, int tablePos)
    {
        Debug.Log(ManaLeft);
        Debug.Log(playedCard.CurrentManaCost);
        Debug.Log("Mana Left after played a creature: " + ManaLeft);
        CreatureLogic newCreature = new CreatureLogic(this, playedCard.ca);
        new PlayACreatureCommand(playedCard.ID, this, tablePos, newCreature).AddToQueue();
        PlayerConnection.SendCommandOnServer((byte)ID, CommandType.PlayACreature, playedCard.ID.ToString(),tablePos.ToString(), playedCard.ca.name);
        ManaLeft -= playedCard.CurrentManaCost;
    }

    public void Die()
    {
        // game over
        // block both players from taking new moves 
        PArea.ControlsON = false;
        otherPlayer.PArea.ControlsON = false;
        TurnManager.Instance.StopTheTimer();
        new GameOverCommand(this).AddToQueue();
    }

    // METHODS TO SHOW GLOW HIGHLIGHTS
    public void HighlightPlayableCards(bool removeAllHighlights = false)
    {
        if (this != GlobalSettings.Instance.LowPlayer)
            return;

       // Debug.Log("HighlightPlayable for player :" + tag);
        foreach (CardLogic cl in hand.CardsInHand)
        {
            GameObject g = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
            if (g!=null)
            {
                g.GetComponent<OneCardManager>().CanBePlayedNow = (cl.CurrentManaCost <= ManaLeft) && !removeAllHighlights;
            }
                
        }

        foreach (CreatureLogic crl in table.CreaturesOnTable)
        {
            GameObject g = IDHolder.GetGameObjectWithID(crl.UniqueCreatureID);
            if(g!= null)
            {
                g.GetComponent<OneCreatureManager>().CanAttackNow = (crl.AttacksLeftThisTurn > 0) && !removeAllHighlights;
                //Debug.Log("creature attack possible" + g.GetComponent<OneCreatureManager>().CanAttackNow);
            }
                
        }
            
        // highlight hero power
        PArea.HeroPower.Highlighted = (!usedHeroPowerThisTurn) && (ManaLeft > 2) && !removeAllHighlights;
    }

    // START GAME METHODS
    public void LoadCharacterInfoFromAsset()
    {
        Health = charAsset.MaxHealth;
        // change the visuals for portrait, hero power, etc...
        PArea.Portrait.characterAsset = charAsset;
        PArea.Portrait.LoadFromAsset();
        // TODO: insert the code to attach hero power script here. 
        if (charAsset.HeroPowerName != null && charAsset.HeroPowerName != "")
        {
            HeroPowerEffect = System.Activator.CreateInstance(System.Type.GetType(charAsset.HeroPowerName)) as SpellEffect;
        }
        else
        {
           // Debug.LogWarning("Check hero powr name for character " + charAsset.ClassName);
        }
    }

    public void TransmitInfoAboutPlayerToVisual()
    {
        PArea.Portrait.gameObject.AddComponent<IDHolder>().UniqueID = PlayerID;
        if (GetComponent<TurnMaker>() is AITurnMaker || GetComponent<TurnMaker>() is Player2TurnMaker) // или is Player2TurnMaker
        {
            // turn off turn making for this character
            PArea.AllowedToControlThisPlayer = false;
        }
        else
        {
            // allow turn making for this character
            PArea.AllowedToControlThisPlayer = true;
        }
    }

    public void UseHeroPower()
    {
        ManaLeft -= 3;
        usedHeroPowerThisTurn = true;
        HeroPowerEffect.ActivateEffect();
        PlayerConnection.SendCommandOnServer((byte)ID, CommandType.UseHeroPower);
    }
}
