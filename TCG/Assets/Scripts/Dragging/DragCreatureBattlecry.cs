using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCreatureBattlecry : DraggingActions
{
    public GameObject Creature;
    private TargetingOptions Targets;
   
    private SpriteRenderer sr;
    private LineRenderer lr;
    private WhereIsTheCardOrCreature whereIsThisCard;
    private VisualStates tempVisualState;
    private Transform triangle;
    private SpriteRenderer triangleSR;
    private GameObject Target;
    private OneCreatureManager Manager;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();
        Manager = GetComponentInParent<OneCreatureManager>();
        whereIsThisCard = GetComponentInParent<WhereIsTheCardOrCreature>();
        Targets = Manager.cardAsset.Targets;
    }

    protected override bool DragSuccessful()
    {
        return true;
    }

    public override void OnStartDrag()
    {
        tempVisualState = whereIsThisCard.VisualState;
        whereIsThisCard.VisualState = VisualStates.Dragging;
        sr.enabled = true;
        lr.enabled = true;
        
        float z = -Camera.main.transform.position.z + transform.position.z;
        var screenMousePos = Input.mousePosition;
        screenMousePos.z = z;

        transform.position = Camera.main.ScreenToWorldPoint(screenMousePos);
        GlobalSettings.Instance.DraggingEnabled = false;

        InfoManager.Instance.ShowTip("Press <ESC> to cancel battlecry");
    }

    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction * 2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            lr.SetPositions(new Vector3[] { transform.parent.position, transform.position - direction * 2.3f });
            lr.enabled = true;

            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f * direction;

            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            lr.enabled = false;
            triangleSR.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            TurnTargetingOff();
    }

    public void TurnTargetingOff()
    {
        GetComponent<DraggableBattlecry>().dragging = false;
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;
        transform.localPosition = new Vector3(0f, 0f, -0.1f);
        whereIsThisCard.VisualState = tempVisualState;
        InfoManager.Instance.HideTip();
        GlobalSettings.Instance.DraggingEnabled = true;
        HoverPreview.PreviewsAllowed = true;
    }

    public override void OnEndDrag()
    {
        Target = null;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 30f);

        bool targetValid = false;

        foreach (RaycastHit h in hits)
        {
            if (h.transform.tag.Contains("Player"))
            {
                Target = h.transform.gameObject;
            }
            else if (h.transform.tag.Contains("Creature"))
            {
                if(h.transform.gameObject == this.gameObject)
                {
                    RaycastHit[] raycasts = Physics.RaycastAll(origin: Camera.main.transform.position,
                        direction: (-Camera.main.transform.position + Creature.transform.position).normalized,
                        maxDistance: 30f);
                    foreach(RaycastHit hit in raycasts)
                    {
                        if (hit.transform.tag.Contains("Creature"))
                            Target = h.transform.parent.gameObject;
                    }

                }
                else
                {
                    Target = h.transform.parent.gameObject;
                }

            }
            
        }
        
        if (Target != null)
        {
            CreatureLogic cl = CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID];
            int targetID = Target.GetComponent<IDHolder>().UniqueID;
            Debug.Log("BattlecryTARGET ID: " + targetID);
            
            switch (Targets)
            {
                case TargetingOptions.AllCharacters:
                    targetValid = true;
                    break;
                case TargetingOptions.AllCreatures:
                    if (Target.tag.Contains("Creature"))
                        targetValid = true;
                    break;
                case TargetingOptions.EnemyCharacters:
                    if (Target.tag.Contains("Creature") || Target.tag.Contains("Player"))
                    {
                        if ((tag.Contains("Low") && Target.tag.Contains("Top"))
                           || (tag.Contains("Top") && Target.tag.Contains("Low")))
                        {
                            targetValid = true;
                        }
                    }
                    break;
                case TargetingOptions.EnemyCreatures:
                    if (Target.tag.Contains("Creature"))
                    {
                        if ((tag.Contains("Low") && Target.tag.Contains("Top"))
                            || (tag.Contains("Top") && Target.tag.Contains("Low")))
                        {
                            targetValid = true;
                        }
                    }
                    break;
                case TargetingOptions.YourCharacters:
                    if (Target.tag.Contains("Creature") || Target.tag.Contains("Player"))
                    {
                        if ((tag.Contains("Low") && Target.tag.Contains("Low"))
                            || (tag.Contains("Top") && Target.tag.Contains("Top")))
                        {
                            targetValid = true;
                        }
                    }
                    break;
                case TargetingOptions.YourCreatures:
                    if (Target.tag.Contains("Creature"))
                    {
                        if ((tag.Contains("Low") && Target.tag.Contains("Low"))
                            || (tag.Contains("Top") && Target.tag.Contains("Top")))
                        {
                            targetValid = true;
                        }
                    }
                    break;
                default:
                    Debug.LogWarning("Reached default case in DragSpellOnTarget! Suspicious behaviour!!");
                    break;
            }

            if (targetValid)
            {
                cl.TriggerBattlecry(targetID);
                TurnTargetingOff();
            }
        }

        if(!targetValid)
            InfoManager.Instance.ShowMessage("Wrong target!", 2f);

    }
    
}
