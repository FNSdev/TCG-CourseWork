using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneCardManager : MonoBehaviour
{
    public CardAsset cardAsset;
    public OneCardManager PreviewManager;

    [Header("Text Component References")]
    public Text NameText;
    public Text ManaCostText;
    public Text DescriptionText;
    public Text HealthText;
    public Text AttackText;

    [Header("ImageReferences")]
    public Image CardTopRibbonImage;
    public Image CardBottomRibbonImage;
    public Image CardGraphicImage;
    public Image CardBodyImage;
    public Image CardFaceFrameImage;
    public Image CardFaceGlowImage;
    public Image CardBackGlowImage;

    void Awake()
    {
        if(cardAsset!=null)
        {
            ReadCardFromAsset();
        }
    }

    public bool CanBePlayedNow = false;

    public void ReadCardFromAsset()
    {
        if(cardAsset.characterAsset != null)
        {
            CardBodyImage.color = cardAsset.characterAsset.ClassCardTint;
            CardFaceFrameImage.color = cardAsset.characterAsset.ClassCardTint;
            CardTopRibbonImage.color = cardAsset.characterAsset.ClassRibbonsTint;
            CardBottomRibbonImage.color = cardAsset.characterAsset.ClassRibbonsTint;
        }
        else
        {
           /* CardBodyImage.color = GlobalSettings.Instance.CardBodyStandartColor;
            CardFaceFrameImage.color = Color.white;
            CardTopRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandartColor;
            CardBottomRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandartColor;*/
        }

        NameText.text = cardAsset.name;
        ManaCostText.text = cardAsset.ManaCost.ToString();
        DescriptionText.text = cardAsset.Description;
        CardGraphicImage.sprite = cardAsset.CardImage;

        if(cardAsset.MaxHealth != 0)
        {
            AttackText.text = cardAsset.Attack.ToString();
            HealthText.text = cardAsset.MaxHealth.ToString();
        }

        if(PreviewManager != null)
        {
            PreviewManager.cardAsset = cardAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }

}
