using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragOnTargetTest : DraggingActionTest
{
    //public TargetingOptions Targets = TargetingOptions.AllCharacters;
    private SpriteRenderer SR;
    private LineRenderer LR;
    private Transform triangle;
    private SpriteRenderer triangleSR;

    void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        LR = GetComponentInChildren<LineRenderer>();
        LR.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();
    }

    public override void OnStartDragging()
    {
        SR.enabled = true;
        LR.enabled = true;
    }

    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float DistanceToTarget = (direction * 2.3f).magnitude;
        if(notNormalized.magnitude > DistanceToTarget)
        {
            LR.SetPositions(new Vector3[] { transform.parent.position, transform.position - direction * 2.3f });
            LR.enabled = true;
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.45f * direction;

            float rotation_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z - 90);
        }
        else
        {
            LR.enabled = false;
            triangleSR.enabled = false;
        }
    }

    public override void OnEndDragging()
    {
        transform.localPosition = new Vector3(0f, 0f, 0.1f);
        SR.enabled = false;
        LR.enabled = false;
        triangleSR.enabled = false;
    }

    protected override bool DragSuccesful()
    {
        return true;
    }
}
