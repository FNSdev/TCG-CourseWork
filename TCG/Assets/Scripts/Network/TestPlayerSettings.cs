using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerSettings : MonoBehaviour {

    public static TestPlayerSettings Instance;
    public List<CardAsset> deck;
    public CharacterAsset characterAsset;

	// Use this for initialization
	void Awake ()
    {
        Instance = this;
	}
	
}
