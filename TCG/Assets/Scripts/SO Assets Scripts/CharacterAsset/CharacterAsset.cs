using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClassOptions
{
    Barbarian,
    Wizard,
    DeathKnigt
}

public class CharacterAsset : ScriptableObject
{
    public ClassOptions Class;
    public int MaxHealth;
    public Sprite AvatarImage;
    public Sprite HeroPowerIconImage;
    public string HeroPowerName;
    public Sprite AvatarBGImage;
    public Sprite HeroPowerBGImage;
    public Color AvatarBGTint;
    public Color HeroPowerBGTint;
    public Color ClassCardTint;
    public Color ClassRibbonsTint;

}