using UnityEngine;
using System.Collections;

public class HeroPowerDrawCardTakeDamage : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        // Take 2 damage
        new DealDamageCommand(TurnManager.Instance.WhoseTurn, 2, TurnManager.Instance.WhoseTurn.Health - 2).AddToQueue();
        // Draw a card
        TurnManager.Instance.WhoseTurn.DrawACard();

    }
}
