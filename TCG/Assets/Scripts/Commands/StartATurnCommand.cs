using UnityEngine;
using System.Collections;

public class StartATurnCommand : Command {

    public StartATurnCommand(Player p)
    {
        CommandSender = p;
    }
    public override void StartCommandExecution()
    {
       // PlayerConnection.SendCommandOnServer((byte)CommandSender.ID, 0);
        TurnManager.Instance.WhoseTurn = CommandSender;
        // this command is completed instantly
        CommandExecutionComplete();
    }
}
