using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToCreature : CreatureEffect {

    public DealDamageToCreature(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount) { }

    public override void Battlecry(ICharacter target)
    {
        new DealDamageCommand(target.ID, specialAmount, target.Health - specialAmount).AddToQueue();
        target.Health -= specialAmount;
    }
              
}
