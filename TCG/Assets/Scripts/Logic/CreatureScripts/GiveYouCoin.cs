using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveYouCoin : CreatureEffect
{
    public GiveYouCoin(Player owner, CreatureLogic creature) : base(owner, creature) { }

    public override void CauseEffect(ICharacter Target = null)
    {
        if(owner.hand.CardsInHand.Count < GlobalSettings.MaxCardsInHand)
        {
            CardLogic newCard = new CardLogic(Collection.Instance.FindAsset("Coin"));
            newCard.owner = owner;
            new GetACardCommand(newCard, owner, fast: true, fromDeck: false).AddToQueue();
            PlayerConnection.SendCommandOnServer((byte)owner.ID, CommandType.GetACard, "Coin");
        }
    }
}
