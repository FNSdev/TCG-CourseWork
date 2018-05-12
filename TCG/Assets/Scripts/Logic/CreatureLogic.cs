using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CreatureLogic: ICharacter 
{
    public delegate void EventHandler(ICharacter target = null);

    public event EventHandler CreatureWasPlayed;
    public event EventHandler CreatureHasDied;
    public event EventHandler TurnEnd;
    public event EventHandler TurnStart;
    public event EventHandler CreatureHasAttacked;
    public event EventHandler OtherCreatureWasPlayed;
    public event EventHandler OtherCreatureHasDied;

    public Player owner;
    public CardAsset ca;
    public int UniqueCreatureID;
    public readonly bool hasBattlecry = false;

    // для хранения пар ID - CreatureLogic
    public static Dictionary<int, CreatureLogic> CreaturesCreatedThisGame = new Dictionary<int, CreatureLogic>();

    public int ID
    {
        get { return UniqueCreatureID; }
    }
    public bool Frozen = false;
    public bool Taunt;
    public bool TargetedBattlecry;

    private int baseHealth;
    private int maxHealth;
   
    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value > 0 ? value : baseHealth; }
    }
        
    private int health;

    public int Health
    {
        get{ return health; }

        set
        {
            if (value > MaxHealth)
                health = MaxHealth;
            else if (value <= 0)
                Die();
            else
                health = value;
        }
    }

    public bool CanAttack
    {
        get
        {
            bool ownersTurn = (TurnManager.Instance.WhoseTurn == owner);
            return (ownersTurn && (AttacksLeftThisTurn > 0) && !Frozen);
        }
    }

    private int baseAttack;
    private int attack;      

    public int Attack
    {
        get { return attack; }
        set { attack = value >= 0 ? value : baseAttack; }
    }
        
    private int attacksForOneTurn = 1;
    public int AttacksLeftThisTurn
    {
        get;
        set;
    }

    public CreatureLogic(Player owner, CardAsset ca)
    {
        this.ca = ca;
        Health = MaxHealth = baseHealth = ca.MaxHealth;
        Attack = baseAttack = ca.Attack;
        attacksForOneTurn = ca.AttacksForOneTurn;
        TargetedBattlecry = ca.TargetedBattlecry;
        
        // AttacksLeftThisTurn сейчас равно 0
        if (ca.Charge)
            AttacksLeftThisTurn = attacksForOneTurn;
        this.owner = owner;
        UniqueCreatureID = IDFactory.GetUniqueID();
        Taunt = ca.Taunt;
        CreatureEffect effect;

        if(ca.BattlecryEffectName != null && ca.BattlecryEffectName != "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.BattlecryEffectName), new System.Object[] { owner, this }) as CreatureEffect;
            hasBattlecry = true;
            CreatureWasPlayed += effect.CauseEffect;
        }
        if (ca.DeathrattleEffectName != null && ca.DeathrattleEffectName != "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.DeathrattleEffectName), new System.Object[] { owner, this }) as CreatureEffect;
            CreatureHasDied += effect.CauseEffect;
        }
        if (ca.TurnEndEffectName != null && ca.TurnEndEffectName != "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.TurnEndEffectName), new System.Object[] { owner, this }) as CreatureEffect;
            TurnEnd += effect.CauseEffect;
        }
        if (ca.TurnStartEffectName != null && ca.TurnStartEffectName != "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.TurnStartEffectName), new System.Object[] { owner, this }) as CreatureEffect;
            TurnStart += effect.CauseEffect;
        }
        if (ca.CreatureAttackedEffectName != null && ca.CreatureAttackedEffectName != "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.CreatureAttackedEffectName), new System.Object[] { owner, this }) as CreatureEffect;
            CreatureHasAttacked += effect.CauseEffect;
        }
        if (ca.OtherCreatureDiedEffectName != null && ca.OtherCreatureDiedEffectName != "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.OtherCreatureDiedEffectName), new System.Object[] { owner, this }) as CreatureEffect;
            OtherCreatureHasDied += effect.CauseEffect;
        }
        if (ca.OtherCreaturePlayedEffectName != null && ca.OtherCreaturePlayedEffectName != "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.OtherCreaturePlayedEffectName), new System.Object[] { owner, this }) as CreatureEffect;
            OtherCreatureWasPlayed += effect.CauseEffect;
        }
        
        CreaturesCreatedThisGame.Add(UniqueCreatureID, this);
    }

    public void OnTurnStart()
    {
        AttacksLeftThisTurn = attacksForOneTurn;
        if (TurnStart != null)
            TurnStart.Invoke();
    }
    public void OnTurnEnd()
    {
        if (TurnEnd != null)
            TurnEnd.Invoke();
    }

    public void Die()
    {   
        owner.table.CreaturesOnTable.Remove(this);
        new CreatureDieCommand(UniqueCreatureID, owner).AddToQueue();
        if (CreatureHasDied != null)
            CreatureHasDied.Invoke();
    }

    public void GoFace()
    {
        AttacksLeftThisTurn--;
        int targetHealthAfter = owner.otherPlayer.Health - Attack;
        new CreatureAttackCommand(owner.otherPlayer, this, 0, Attack, Health, targetHealthAfter).AddToQueue();
        if (CreatureHasAttacked != null)
            CreatureHasAttacked.Invoke();
    }

    public void AttackCreature (CreatureLogic target)
    {
        AttacksLeftThisTurn--;
        // calculate the values so that the creature does not fire the DIE command before the Attack command is sent
        int targetHealthAfter = target.Health - Attack;
        int attackerHealthAfter = Health - target.Attack;
        new CreatureAttackCommand(target, this, target.Attack, Attack, attackerHealthAfter, targetHealthAfter).AddToQueue();
        if (CreatureHasAttacked != null)
            CreatureHasAttacked.Invoke();
    }

    public void AttackCreatureWithID(int uniqueCreatureID)
    {
        CreatureLogic target = CreatureLogic.CreaturesCreatedThisGame[uniqueCreatureID];
        AttackCreature(target);
    }

    public void TriggerBattlecry(int id)
    {
        if (id < 0)
            TriggerBattlecry();
        else if (id == owner.ID)
            TriggerBattlecry(owner);
        else if (id == owner.otherPlayer.ID)
            TriggerBattlecry(owner.otherPlayer.ID);
        else
            TriggerBattlecry(CreaturesCreatedThisGame[id]);
    }

    public void TriggerBattlecry(ICharacter Target = null)
    {
        CreatureWasPlayed.Invoke(Target);
    }

}
