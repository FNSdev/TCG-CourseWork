using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableTest2 : MonoBehaviour
{
    public bool UsePointerDisplacement = true;
    private DraggingActionTest DA;
    private bool dragging = false;
    private Vector3 pointerDisplacement = Vector3.zero;
    private float zDisplacement;

    void Awake()
    {
        DA = GetComponent<DraggingActionTest>();
    }

    void OnMouseDown()
    {
        if(DA.CanDrag)
        {
            dragging = true;
            HoverPreview.PreviewsAllowed = false;
            DA.OnStartDragging();
            zDisplacement = -Camera.main.transform.position.z + transform.position.z;
            if (UsePointerDisplacement)
            {
                pointerDisplacement = -transform.position + MouseInWorldCoords();
            }
            else
            {
                pointerDisplacement = Vector3.zero;
            }
        }
    }

    void Update()
    {
        if(dragging)
        {
            Vector3 mousePosition = MouseInWorldCoords();
            DA.OnDraggingInUpdate(); //нужен, например, для отображения стрелки выбора цели при атаки/разыгрывании карты заклинания
            transform.position = new Vector3(mousePosition.x - pointerDisplacement.x, mousePosition.y - pointerDisplacement.y, transform.position.z);
        }
    }

    void OnMouseUp()
    {
        if(dragging)
        {
            dragging = false;
            HoverPreview.PreviewsAllowed = true;
            DA.OnEndDragging();
        }
    }

    private Vector3 MouseInWorldCoords()
    {
        Vector3 screenMousePosition = Input.mousePosition;
        screenMousePosition.z = zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePosition);
    }
}
