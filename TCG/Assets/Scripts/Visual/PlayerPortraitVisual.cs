using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerPortraitVisual : MonoBehaviour
{
    public CharacterAsset characterAsset;

    [Header("Text component references")]
    public Text HealthText;

    [Header("Image component references")]
    public Image HeroPowerIconImage;
    public Image HeroPowerBGImage;
    public Image PortretImage;
    public Image PortretBGImage;

    void Awake()
    {
        if(characterAsset!=null)
        {
            LoadFromAsset();
        }
    }

    public void LoadFromAsset()
    {
        HealthText.text = characterAsset.MaxHealth.ToString();
        HeroPowerIconImage.sprite = characterAsset.HeroPowerIconImage;
        HeroPowerBGImage.sprite = characterAsset.HeroPowerBGImage;
        PortretBGImage.sprite = characterAsset.AvatarBGImage;
        PortretImage.sprite = characterAsset.AvatarImage;
        HeroPowerBGImage.color = characterAsset.HeroPowerBGTint;
        PortretBGImage.color = characterAsset.AvatarBGTint;
    }

    public void TakeDamage(int amount, int HealthAfter)
    {
        if(amount > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount);
            HealthText.text = HealthAfter.ToString();
        }
    }

    public void Explode()
    {
        // TODO
         /* Instantiate(GlobalSettings.Instance.ExplosionPrefab, transform.position, Quaternion.identify);
          Sequence s = DOTween.Sequence();
          s.PrependInterval(2f);
          s.OnComplete(() => GlobalSettings.Instance.GameOverCanvas.SetActive(true);
          */
    }
}
