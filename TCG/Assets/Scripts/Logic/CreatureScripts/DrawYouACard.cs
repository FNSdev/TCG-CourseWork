using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawYouACard : CreatureEffect
{
    public DrawYouACard(Player owner, CreatureLogic creature) : base(owner, creature) { }

    public override void CauseEffect(ICharacter Target = null)
    {
        /*if(owner.deck.cards.Count > 0 && owner.hand.CardsInHand.Count < GlobalSettings.MaxCardsInHand)
        {
            owner.DrawACard();
            PlayerConnection.SendCommandOnServer((byte)owner.ID, CommandType.DrawACard, "0");
        }*/

        if (owner.DrawACard())
            PlayerConnection.SendCommandOnServer((byte)owner.ID, CommandType.DrawACard, "0");

        /*while(true)
        {
            if(!Command.CardDrawPending())
            {
                if (owner.DrawACard())
                    PlayerConnection.SendCommandOnServer((byte)owner.ID, CommandType.DrawACard, "0");
                break;
            }
        }*/

    }
}
