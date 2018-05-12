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

        foreach(CreatureLogic CL in wisps)
        {
            if (owner.otherPlayer.table.CreaturesOnTable.Count < GlobalSettings.MaxCreaturesOnTable)
            {
                owner.otherPlayer.table.CreaturesOnTable.Insert(0, CL);
                owner.otherPlayer.PArea.tableVisual.AddCreatureAtIndex(Swampler, CL.UniqueCreatureID, 0, false);
            }
        }
    }
}
