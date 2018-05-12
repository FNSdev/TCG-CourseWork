using UnityEngine;
using System.Collections;

public class PlayASpellCardCommand: Command
{
    private CardLogic card;
    private Player p;
    //private ICharacter target;

    public PlayASpellCardCommand(Player p, CardLogic card)
    {
        this.card = card;
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        // move this card to the spot
        p.hand.CardsInHand.Remove(card);
        p.PArea.handVisual.PlayASpellFromHand(card.UniqueCardID);
        p.HighlightPlayableCards();
        // do all the visual stuff (for each spell separately????)
    }
}
