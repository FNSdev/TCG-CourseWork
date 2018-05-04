using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HoverPreview : MonoBehaviour
{
    public GameObject TurnThisOffWhenPreviewing;
    public Vector3 TargetPosition;
    public float TargetScale;
    public GameObject PreviewGameObject;
    public bool ActivateInAwake = false;

    private static HoverPreview CurrentlyViewing = null;
    private static bool _PreviewsAllowed = true;
    private bool _ThisPreviewEnabled = false;

    public bool OverCollider { get; set; }

    public static bool PreviewsAllowed
    {
        get { return _PreviewsAllowed; }
        set
        {
            _PreviewsAllowed = value;
            if (!_PreviewsAllowed) StopAllPreviews();
        }
    }

    public bool ThisPreviewEnabled
    {
        get { return _ThisPreviewEnabled; }
        set
        {
            _ThisPreviewEnabled = value;
            if (!_ThisPreviewEnabled) StopThisPreview();
        }
    }

    void Awake()
    {
        ThisPreviewEnabled = ActivateInAwake;
    }

    void OnMouseEnter()
    {
        OverCollider = true;
        if (PreviewsAllowed && ThisPreviewEnabled)
        {
            PreviewThisObject();
        }
    }

    void OnMouseExit()
    {
        OverCollider = false;
        if(!PreviewingSomeCard())
        {
            StopAllPreviews();
        }
    }

    void PreviewThisObject()
    {
        StopAllPreviews();

        CurrentlyViewing = this;
        PreviewGameObject.SetActive(true);

        if (TurnThisOffWhenPreviewing != null)
        {
            TurnThisOffWhenPreviewing.SetActive(false);
        }

        PreviewGameObject.transform.localPosition = Vector3.zero; //local устанавливает позицию относительно объекта родителя
        PreviewGameObject.transform.localScale = Vector3.one;
        PreviewGameObject.transform.DOLocalMove(TargetPosition, 1f).SetEase(Ease.OutQuint);
        PreviewGameObject.transform.DOScale(TargetScale, 1f).SetEase(Ease.OutQuint);
    }

    private void StopThisPreview()
    {
        PreviewGameObject.SetActive(false);
        PreviewGameObject.transform.localPosition = Vector3.zero;
        PreviewGameObject.transform.localScale = Vector3.one;
        if (TurnThisOffWhenPreviewing != null)
        {
            TurnThisOffWhenPreviewing.SetActive(true);
        }
    }

    private static void StopAllPreviews()
    {
        if(CurrentlyViewing!=null)
        {
            CurrentlyViewing.StopThisPreview();
        }
    }

    private static bool PreviewingSomeCard()
    {
        if (!_PreviewsAllowed) return false;

        HoverPreview[] AllHoverBlowups = GameObject.FindObjectsOfType<HoverPreview>();
        
        foreach(HoverPreview HB in AllHoverBlowups)
        {
            if (HB.OverCollider && HB.ThisPreviewEnabled) return true;
        }

        return false;
    }
}


