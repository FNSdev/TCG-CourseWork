using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour {

    public List<CardAsset> Assets = new List<CardAsset>();

    public static Collection Instance;

    void Awake()
    {
        Instance = this;
    }

    public CardAsset FindAsset(string creatureName)
    {
        foreach(CardAsset CA in Assets)
        {
            if (CA.name == creatureName)
                return CA;
        }
        return null;
    }

}
