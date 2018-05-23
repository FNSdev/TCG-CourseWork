using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTheTimerCommand : Command {

    public RopeTimer timer;

    public StartTheTimerCommand(RopeTimer timer)
    {
        this.timer = timer;
    }

    public override void StartCommandExecution()
    {
        timer.StartTimer();
        CommandExecutionComplete();
    }
}
