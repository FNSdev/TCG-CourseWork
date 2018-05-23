using UnityEngine;
using System.Collections;

public class HeroPowerDrawCardTakeDamage : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        PlayerConnection.SendCommandOnServer((byte)TurnManager.Instance.WhoseTurn.ID, CommandType.DealDamage, TurnManager.Instance.WhoseTurn.ID.ToString(), "3", (TurnManager.Instance.WhoseTurn.Health - 3).ToString());
        new DealDamageCommand(TurnManager.Instance.WhoseTurn.ID, 3, TurnManager.Instance.WhoseTurn.Health - 3).AddToQueue();
        if(TurnManager.Instance.WhoseTurn.deck.cards.Count > 0)
            PlayerConnection.SendCommandOnServer((byte)TurnManager.Instance.WhoseTurn.ID, CommandType.DrawACard, "0");
        TurnManager.Instance.WhoseTurn.DrawACard();
        //PlayerConnection.SendCommandOnServer((byte)TurnManager.Instance.WhoseTurn.ID, CommandType.DrawACard, "0");
    }
}
