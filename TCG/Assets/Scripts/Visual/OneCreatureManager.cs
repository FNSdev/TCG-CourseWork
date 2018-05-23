using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneCreatureManager : MonoBehaviour
{
    public CardAsset cardAsset;
    public OneCardManager PreviewManager;

    [Header("Text component references")]
    public Text HealthText;
    public Text AttackText;

    [Header("Image references")]
    public Image CreatureGraphicImage;
    public Image CreatureGlowImage;
    public Image TauntImage;

    private bool _CanAttackNow = false;
    public bool CanAttackNow
    {
        get { return _CanAttackNow; }
        set
        {
            CreatureGlowImage.enabled = value;
            _CanAttackNow = value;
        }
    }

    void Awake()
    {
        if(cardAsset!=null)
        {
            ReadCreatureFromAsset();
        }

    }

    public void ReadCreatureFromAsset()
    {
        CreatureGraphicImage.sprite = cardAsset.CardImage;
        AttackText.text = cardAsset.Attack.ToString();
        HealthText.text = cardAsset.MaxHealth.ToString();

        if(PreviewManager!=null)
        {
            PreviewManager.cardAsset = cardAsset; 
        }

        TauntImage.enabled = cardAsset.Taunt;
    }

    public void TakeDamage(int amount, int healthAfter)
    {
        if(amount>0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount);
            HealthText.text = healthAfter.ToString();
        }
    }

}
