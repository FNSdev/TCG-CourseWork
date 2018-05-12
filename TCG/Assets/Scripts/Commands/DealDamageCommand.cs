using UnityEngine;
using System.Collections;

public class DealDamageCommand : Command {

    private ICharacter Target;
    private int Amount;
    private int HealthAfter;

    public DealDamageCommand(ICharacter target, int amount, int healthAfter)
    {
        Target = target;
        Amount = amount;
        HealthAfter = healthAfter;
    }

    public override void StartCommandExecution()
    {
        Debug.Log("In deal damage command!");

        GameObject target = IDHolder.GetGameObjectWithID(Target.ID);
        if (Target.ID == GlobalSettings.Instance.LowPlayer.PlayerID || Target.ID == GlobalSettings.Instance.TopPlayer.PlayerID)
        {
            // target is a hero
            target.GetComponent<PlayerPortraitVisual>().TakeDamage(Amount, HealthAfter);
        }
        else
        {
            // target is a creature
            target.GetComponent<OneCreatureManager>().TakeDamage(Amount, HealthAfter);
        }

        new DelayCommand(1.3f).AddToQueue();
        Target.Health = HealthAfter;
        CommandExecutionComplete();
    }
}
