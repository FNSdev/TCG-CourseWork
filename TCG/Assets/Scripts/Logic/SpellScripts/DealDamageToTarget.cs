using UnityEngine;
using System.Collections;

public class DealDamageToTarget : SpellEffect 
{    
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        new DealDamageCommand(target, specialAmount, healthAfter: target.Health - specialAmount).AddToQueue();
    }
}
