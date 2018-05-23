using UnityEngine;
using System.Collections;

public class CreatureAttackCommand : Command 
{
    private int AttackerHealthAfter;
    private int TargetHealthAfter;
    private int DamageTakenByAttacker;
    private int DamageTakenByTarget;
    private int AttackerID;
    private int TargetID;

    public CreatureAttackCommand(int targetID, int attackerID, int damageTakenByAttacker, int damageTakenByTarget, int attackerHealthAfter, int targetHealthAfter)
    {
        TargetID = targetID;
        AttackerID = attackerID;
        AttackerHealthAfter = attackerHealthAfter;
        TargetHealthAfter = targetHealthAfter;
        DamageTakenByTarget = damageTakenByTarget;
        DamageTakenByAttacker = damageTakenByAttacker;
    }

    public override void StartCommandExecution()
    {
        GameObject AttackerObject = IDHolder.GetGameObjectWithID(AttackerID);
        AttackerObject.GetComponent<CreatureAttackVisual>().AttackTarget(TargetID, DamageTakenByTarget, DamageTakenByAttacker, AttackerHealthAfter, TargetHealthAfter);

        ICharacter Target, Attacker;
        Attacker = CreatureLogic.CreaturesCreatedThisGame[AttackerID];

        if (TargetID == 1)
            Target = GlobalSettings.Instance.TopPlayer;
        else if (TargetID == 2)
            Target = GlobalSettings.Instance.LowPlayer;
        else
            Target = CreatureLogic.CreaturesCreatedThisGame[TargetID];

        Target.Health = TargetHealthAfter;
        Attacker.Health = AttackerHealthAfter;
    }
}
