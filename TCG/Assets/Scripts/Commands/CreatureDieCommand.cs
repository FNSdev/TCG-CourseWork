using UnityEngine;
using System.Collections;

public class CreatureDieCommand : Command 
{
    private int DeadCreatureID;

    public CreatureDieCommand(int CreatureID, Player p)
    {
        CommandSender = p;
        this.DeadCreatureID = CreatureID;
    }

    public override void StartCommandExecution()
    {
        CommandSender.PArea.tableVisual.RemoveCreatureWithID(DeadCreatureID);
    }
}
