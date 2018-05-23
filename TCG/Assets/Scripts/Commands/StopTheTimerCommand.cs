using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTheTimerCommand : Command {

    public RopeTimer timer;

    public StopTheTimerCommand(RopeTimer timer)
    {
        this.timer = timer;
    }

    public override void StartCommandExecution()
    {
        timer.StopTimer();
        CommandExecutionComplete();
    }
}
