using UnityEngine;
using System.Collections;

public class HeroPower2Face : SpellEffect 
{

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        new DealDamageCommand(TurnManager.Instance.WhoseTurn.otherPlayer.ID, 2, TurnManager.Instance.WhoseTurn.otherPlayer.Health - 2).AddToQueue();
    }
}
