using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableBattlecry : MonoBehaviour {

    public bool dragging = false;

    // distance from the center of this Game Object to the point where we clicked to start dragging 
    private Vector3 pointerDisplacement;

    // distance from camera to mouse on Z axis 
    private float zDisplacement;
    private DragCreatureBattlecry battlecryDA;

    void Awake()
    {
        battlecryDA = GetComponent<DragCreatureBattlecry>();
    }
    
    void Update()
    {
        if (dragging)
        {
            Vector3 mousePos = MouseInWorldCoords();
            transform.position = new Vector3(mousePos.x - pointerDisplacement.x, mousePos.y - pointerDisplacement.y, transform.position.z);
            battlecryDA.OnDraggingInUpdate();
            if(Input.GetMouseButtonDown(0))
            {
                battlecryDA.OnEndDrag();
            }
        }
    }

    public void ActivateDragging()
    {
        if (battlecryDA != null) 
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
        screenMousePos.z = zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }
}
