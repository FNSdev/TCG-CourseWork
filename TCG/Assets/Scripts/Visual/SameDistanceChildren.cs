using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameDistanceChildren : MonoBehaviour
{
    public Transform[] Children;

    void Awake()
    {
        Vector3 FirstElementPosition = Children[0].transform.position;
        Vector3 LastElementPosition = Children[Children.Length - 1].transform.position;

        float XDistance = (LastElementPosition.x - FirstElementPosition.x) / (float)(Children.Length - 1);
        float YDistance = (LastElementPosition.y - FirstElementPosition.y) / (float)(Children.Length - 1);
        float ZDistance = (LastElementPosition.z - FirstElementPosition.z) / (float)(Children.Length - 1);

        Vector3 Distance = new Vector3(XDistance, YDistance, ZDistance);

        for(int i = 1; i < Children.Length; i++)
        {
            Children[i].transform.position = Children[i - 1].transform.position + Distance;
        }
    }
}
