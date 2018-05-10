using UnityEngine;
using System.Collections;

public abstract class CreatureEffect 
{
    protected Player owner;
    protected CreatureLogic creature;

    public CreatureEffect(Player owner, CreatureLogic creature)
    {
        this.creature = creature;
        this.owner = owner;
    }

    public virtual void CauseEffect(ICharacter Target = null) { }

}
