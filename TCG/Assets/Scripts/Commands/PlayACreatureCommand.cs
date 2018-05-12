using UnityEngine;
using System.Collections;

public class PlayACreatureCommand : Command
{
    private CardLogic cl;
    private int tablePos;
    private Player p;
    private CreatureLogic Creature;

    public PlayACreatureCommand(CardLogic cl, Player p, int tablePos, CreatureLogic creature)
    {
        this.p = p;
        this.cl = cl;
        this.tablePos = tablePos;
        Creature = creature;
    }

    public override void StartCommandExecution()
    {
        // remove and destroy the card in hand 
        HandVisual PlayerHand = p.PArea.handVisual;
        GameObject card = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
        PlayerHand.RemoveCard(card);
        GameObject.Destroy(card);
        // enable Hover Previews Back
        HoverPreview.PreviewsAllowed = true;
        // move this card to the spot 

        p.hand.CardsInHand.Remove(cl);
        p.table.CreaturesOnTable.Insert(tablePos, Creature);

        p.PArea.tableVisual.AddCreatureAtIndex(cl.ca, Creature.ID, tablePos);
        p.HighlightPlayableCards();
    }
}
