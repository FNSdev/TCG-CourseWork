using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetingOptions
{
    NoTarget,
    AllCreatures,
    EnemyCreatures,
    YourCreatures,
    AllCharacters,
    EnemyCharacters,
    YourCharacters
}

public class CardAsset : ScriptableObject
{
    [Header("General info")]
    public CharacterAsset characterAsset; //если null, карта нейтральная
    [TextArea(2, 3)]
    public string Description;
    public Sprite CardImage;
    public int ManaCost;
    public TargetingOptions Targets;

    [Header("Creature info")]
    public int MaxHealth; //если = 0, значит, это карта заклинания
    public int Attack;
    public int AttacksForOneTurn = 1;
    public bool Charge;
    public bool Taunt;
    public bool TargetedBattlecry;
    public string BattlecryEffectName;
    public string DeathrattleEffectName;
    public string TurnEndEffectName;
    public string TurnStartEffectName;
    public string OtherCreaturePlayedEffectName;
    public string OtherCreatureDiedEffectName;
    public string CreatureAttackedEffectName;
    public string CardWasDrawnEffectName;
    
    [Header("Spell info")]
    public string SpellScriptName;
    public int specialSpellAmount;
    
}

