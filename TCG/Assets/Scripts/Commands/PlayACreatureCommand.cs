using UnityEngine;
using System.Collections;

public class PlayACreatureCommand : Command
{
    private int cardID;
    private int tablePos;
    private Player p;
    private CreatureLogic Creature;

    public PlayACreatureCommand(int cardID, Player p, int tablePos, CreatureLogic creature)
    {
        this.p = p;
        this.cardID = cardID;
        this.tablePos = tablePos;
        Creature = creature;
    }

    public override void StartCommandExecution()
    {
        // remove and destroy the card in hand 
        HandVisual PlayerHand = p.PArea.handVisual;
        GameObject card = IDHolder.GetGameObjectWithID(cardID);
        PlayerHand.RemoveCard(card);
        GameObject.Destroy(card);
        // enable Hover Previews Back
        HoverPreview.PreviewsAllowed = true;
        // move this card to the spot 

        p.hand.CardsInHand.Remove(CardLogic.CardsCreatedThisGame[cardID]);
        p.table.CreaturesOnTable.Insert(tablePos, Creature);
        p.PArea.tableVisual.AddCreatureAtIndex(CardLogic.CardsCreatedThisGame[cardID].ca, Creature.ID, tablePos);
        p.HighlightPlayableCards();
    }
}
