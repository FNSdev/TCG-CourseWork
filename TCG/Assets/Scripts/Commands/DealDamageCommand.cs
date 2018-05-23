using UnityEngine;
using System.Collections;

public class DealDamageCommand : Command {

    private int Amount;
    private int HealthAfter;
    private int TargetID;

    public DealDamageCommand(int targetID, int amount, int healthAfter)
    {
        TargetID = targetID;
        Amount = amount;
        HealthAfter = healthAfter;
    }

    public override void StartCommandExecution()
    {
        Debug.Log("In deal damage command!");

        GameObject target = IDHolder.GetGameObjectWithID(TargetID);
        ICharacter Target;
        if (TargetID == GlobalSettings.Instance.LowPlayer.PlayerID || TargetID == GlobalSettings.Instance.TopPlayer.PlayerID)
        {
            Target = TargetID == 1 ? GlobalSettings.Instance.TopPlayer : GlobalSettings.Instance.LowPlayer;
            target.GetComponent<PlayerPortraitVisual>().TakeDamage(Amount, HealthAfter);
        }
        else
        {
            Target = CreatureLogic.CreaturesCreatedThisGame[TargetID];
            target.GetComponent<OneCreatureManager>().TakeDamage(Amount, HealthAfter);
        }

        new DelayCommand(1.3f).AddToQueue();
        Target.Health = HealthAfter;
        CommandExecutionComplete();
    }
}
