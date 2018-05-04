using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DraggingActionTestReturner : DraggingActionTest
{
    private Vector3 savedPosition;

    public override void OnDraggingInUpdate()
    {
        
    }

    public override void OnEndDragging()
    {
      // transform.DOMove(savedPosition, 1f);
       transform.DOMove(savedPosition, 1f).SetEase(Ease.OutQuint); 
    }

    public override void OnStartDragging()
    {
        savedPosition = transform.position;
    }

    protected override bool DragSuccesful()
    {
        return true;
    }
}
