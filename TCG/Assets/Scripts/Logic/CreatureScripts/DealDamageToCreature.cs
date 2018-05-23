using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToCreature : CreatureEffect {

    public DealDamageToCreature(Player owner, CreatureLogic creature) : base(owner, creature) { }

    private int _damage = 3;
    public int Damage
    {
        get { return _damage; }
        set { _damage = value > 0 ? value : 0; }
    }
    public override void CauseEffect(ICharacter target)
    {
        PlayerConnection.SendCommandOnServer((byte)owner.ID, CommandType.DealDamage, target.ID.ToString(), _damage.ToString(), (target.Health - _damage).ToString());
        new DealDamageCommand(target.ID, _damage, target.Health - _damage).AddToQueue();
    }
              
}
