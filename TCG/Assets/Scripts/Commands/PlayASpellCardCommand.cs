using UnityEngine;
using System.Collections;

public class PlayASpellCardCommand: Command
{
    private int cardID;

    public PlayASpellCardCommand(Player p, int cardID)
    {
        this.cardID = cardID;
        CommandSender = p;
    }

    public override void StartCommandExecution()
    {
        // move this card to the spot
        CommandSender.hand.CardsInHand.Remove(CardLogic.CardsCreatedThisGame[cardID]);
        CommandSender.PArea.handVisual.PlayASpellFromHand(cardID);
        CommandSender.HighlightPlayableCards();
        // do all the visual stuff (for each spell separately????)
    }
}
