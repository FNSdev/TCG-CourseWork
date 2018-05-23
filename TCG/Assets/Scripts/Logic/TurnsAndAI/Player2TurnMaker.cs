using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2TurnMaker : TurnMaker
{
    public override void OnTurnStart()
    {
        p.OnTurnStart();
        new ShowMessageCommand("Your opponent`s turn!", 2.0f).AddToQueue();
        p.DrawACard();
    }
}
