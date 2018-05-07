using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableBattlecry : MonoBehaviour {

    public bool dragging = false;

    // distance from the center of this Game Object to the point where we clicked to start dragging 
    private Vector3 pointerDisplacement;

    // distance from camera to mouse on Z axis 
    private float zDisplacement;

    // reference to DraggingActions script. Dragging Actions should be attached to the same GameObject.
    private DragCreatureBattlecry battlecryDA;


    // STATIC property that returns the instance of Draggable that is currently being dragged

    // MONOBEHAVIOUR METHODS
    void Awake()
    {
        battlecryDA = GetComponent<DragCreatureBattlecry>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            Vector3 mousePos = MouseInWorldCoords();
            //Debug.Log(mousePos);
            transform.position = new Vector3(mousePos.x - pointerDisplacement.x, mousePos.y - pointerDisplacement.y, transform.position.z);
            battlecryDA.OnDraggingInUpdate();
            if(Input.GetMouseButtonDown(0))
            {
                HoverPreview.PreviewsAllowed = true;
                battlecryDA.OnEndDrag();
            }
        }
    }

    public void ActivateDragging()
    {
        if (battlecryDA != null && CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].effect != null)
        {
            dragging = true;
            // when we are dragging something, all previews should be off
            HoverPreview.PreviewsAllowed = false;
            battlecryDA.OnStartDrag();
            zDisplacement = -Camera.main.transform.position.z + transform.position.z;
            pointerDisplacement = -transform.position + MouseInWorldCoords();
        }
    }

    // returns mouse position in World coordinates for our GameObject to follow. 
    public Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        //Debug.Log(screenMousePos);
        screenMousePos.z = zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }
}
