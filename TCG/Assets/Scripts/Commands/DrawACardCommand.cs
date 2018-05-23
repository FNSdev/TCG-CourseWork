using UnityEngine;
using System.Collections;

public class DrawACardCommand : Command
{
    private bool fast;
    private int ID;
    private bool fromDeck;
    public DrawACardCommand(Player p, bool fast, bool fromDeck)
    {
        CommandSender = p;
        this.fast = fast;
        this.fromDeck = fromDeck;
    }
    public override void StartCommandExecution()
    {
        CardLogic newCard = new CardLogic(CommandSender.deck.cards[0]);
        newCard.owner = CommandSender;
        CommandSender.PArea.PDeck.CardsInDeck--;
        CommandSender.PArea.handVisual.GivePlayerACard(newCard.ca, newCard.UniqueCardID, fast, fromDeck);
        CommandSender.hand.CardsInHand.Add(newCard);
        if(fromDeck)
            CommandSender.deck.cards.RemoveAt(0);
    }
}
