using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamageEffect : MonoBehaviour
{
    public Sprite[] Splashes;
    public Image DamageImage;
    public CanvasGroup CG;
    public Text DamageText;

    void awake()
    {
        DamageImage.sprite = Splashes[Random.Range(0, Splashes.Length)];
    }

    private IEnumerator ShowDamageEffect()
    {
        CG.alpha = 1f;
        yield return new WaitForSeconds(1f);
        while(CG.alpha > 0)
        {
            CG.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);   
        }
        Destroy(this.gameObject);
    }

    public static void CreateDamageEffect(Vector3 position, int amount)
    {
        GameObject newDamageEffect = GameObject.Instantiate(GlobalSettings.Instance.DamageEffectPrefab, position, Quaternion.identity) as GameObject;
        DamageEffect DE = newDamageEffect.GetComponent<DamageEffect>();
        DE.DamageText.text = "-" + amount.ToString();
        DE.StartCoroutine(DE.ShowDamageEffect());
    }
}


