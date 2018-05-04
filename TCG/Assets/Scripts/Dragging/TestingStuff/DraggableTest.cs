using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableTest : MonoBehaviour
{

    public bool UsePointerDisplacement = true;
    private bool dragging = false;
    private Vector3 pointerDisplacement = Vector3.zero; //расстояние от центра объекта до точки, куда мы нажали, чтобы начать перетаскивание
    private float zDisplacement; //расстояние от камеры до курсора по оси Z

    void OnMouseDown()
    {
        dragging = true;
        zDisplacement = -Camera.main.transform.position.z + transform.position.z;
        if(UsePointerDisplacement)
        {
            pointerDisplacement = -transform.position + MouseInWorldCoords();
        }
        else
        {
            pointerDisplacement = Vector3.zero;
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if(dragging)
        {
            Vector3 mousePosition = MouseInWorldCoords();
            transform.position = new Vector3(mousePosition.x - pointerDisplacement.x, mousePosition.y - pointerDisplacement.y, transform.position.z);
        }
		
	}

    void OnMouseUp()
    {
        if(dragging)
        {
            dragging = false;
        }
    }

    private Vector3 MouseInWorldCoords()
    {
        var screenMousePosition = Input.mousePosition;
        screenMousePosition.z = zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePosition);
    }
}
