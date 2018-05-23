using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon2SwamplersForOpponent : CreatureEffect {
    public Summon2SwamplersForOpponent(Player owner, CreatureLogic creature) : base(owner, creature) { }

    public override void CauseEffect(ICharacter Target = null)
    {
        CardAsset Swampler = Collection.Instance.FindAsset("Swampler");
        if (Swampler == null)
            Debug.Log("OOOPS");
        CreatureLogic[] wisps = { new CreatureLogic(owner.otherPlayer, Swampler), new CreatureLogic(owner.otherPlayer, Swampler) };
        int AmountOfCreatures = owner.otherPlayer.table.CreaturesOnTable.Count;

        for(int i = 0; i < 2; i++)
        {
            if (AmountOfCreatures < GlobalSettings.MaxCreaturesOnTable)
            {
                new SummonACreatureCommand(owner.otherPlayer, 0, wisps[i]).AddToQueue();
                AmountOfCreatures++;
            }
            else
                break;
        }
    }
}
