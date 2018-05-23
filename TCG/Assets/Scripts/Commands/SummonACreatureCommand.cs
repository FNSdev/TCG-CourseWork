using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonACreatureCommand : Command
{
    private int tablePos;
    private Player player;
    private CreatureLogic creature;

    public SummonACreatureCommand(Player player, int tablePos, CreatureLogic creature)
    {
        this.player = player;
        this.tablePos = tablePos;
        this.creature = creature;
    }

    public override void StartCommandExecution()
    {
        player.table.CreaturesOnTable.Insert(tablePos, creature);
        player.PArea.tableVisual.AddCreatureAtIndex(CreatureLogic.CreaturesCreatedThisGame[creature.ID].ca, creature.ID, tablePos);
    }
}
