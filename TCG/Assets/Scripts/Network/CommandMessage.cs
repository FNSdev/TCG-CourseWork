using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum CommandType : byte
{
    Initialize = 0,
    StartATurn,
    DrawACard,
    GetACard,
    UpdateManaBar,
    PlayACreature,
    SummonACreature,
    AttackCreature,
    PlayASpell,
    UseHeroPower,
    DealDamage
}

public class MessageType
{
    public static short Command = MsgType.Highest + 1;
    public static short DeckInfo = (short)((int)Command + 1);
}

public class CommandMessage : MessageBase
{
    public byte Player; // 1 - player1, 2 - player2
    public short connectionID;
    public CommandType Command;
    public string[] options;
}

public class DeckInfoMessage : MessageBase
{
    public byte Player;
    public short connectionID;
    public string[] Assets;
}