using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CardRotation : MonoBehaviour {

    public RectTransform CardFront;
    public RectTransform CardBack;
    public Transform targetFacePoint;
    public Collider col;
    private bool showingBack = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    /*  Направляем луч из камеры к точке, немного выступающей вперед относительно передней части карты.
     *  Если луч проходит через коллайдер, расположенный между этой точкой и передней частью карты, показываем заднюю часть карты.
     *  
     *  normalized - возвращает этот vector3 нормальной(единичной) длины
     *  magnitude - длина vector3
     * 
    */
	void Update ()    
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin: Camera.main.transform.position, direction: (-Camera.main.transform.position + targetFacePoint.position).normalized,
            maxDistance: (-Camera.main.transform.position + targetFacePoint.position).magnitude);

        bool passedThroughColliderOnCard = false;
        foreach(RaycastHit hit in hits)
        {
            if(hit.collider == col)
            {
                passedThroughColliderOnCard = true;
            }
        }

        if(passedThroughColliderOnCard != showingBack)
        {
            showingBack = passedThroughColliderOnCard;
            if(showingBack)
            {
                CardFront.gameObject.SetActive(false);
                CardBack.gameObject.SetActive(true);
            }
            else
            {
                CardBack.gameObject.SetActive(false);
                CardFront.gameObject.SetActive(true);
            }
        }

	}
}
