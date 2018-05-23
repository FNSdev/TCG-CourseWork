using UnityEngine;
using System.Collections;

public class PlayerTurnMaker : TurnMaker 
{
    public override void OnTurnStart()
    {
        p.OnTurnStart();
        // dispay a message that it is player`s turn
        new ShowMessageCommand("Your Turn!", 2.0f).AddToQueue();
        p.DrawACard();
    }
}
