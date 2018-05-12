using UnityEngine;
using System.Collections;

public class CreatureAttackCommand : Command 
{
    private ICharacter Target;
    private ICharacter Attacker;
    private int AttackerHealthAfter;
    private int TargetHealthAfter;
    private int DamageTakenByAttacker;
    private int DamageTakenByTarget;

    public CreatureAttackCommand(ICharacter target, ICharacter attacker, int damageTakenByAttacker, int damageTakenByTarget, int attackerHealthAfter, int targetHealthAfter)
    {
        Target = target;
        Attacker = attacker;
        AttackerHealthAfter = attackerHealthAfter;
        TargetHealthAfter = targetHealthAfter;
        DamageTakenByTarget = damageTakenByTarget;
        DamageTakenByAttacker = damageTakenByAttacker;
    }

    public override void StartCommandExecution()
    {
        GameObject AttackerObject = IDHolder.GetGameObjectWithID(Attacker.ID);
        AttackerObject.GetComponent<CreatureAttackVisual>().AttackTarget(Target.ID, DamageTakenByTarget, DamageTakenByAttacker, AttackerHealthAfter, TargetHealthAfter);

        Target.Health = TargetHealthAfter;
        Attacker.Health = AttackerHealthAfter;
    }
}
