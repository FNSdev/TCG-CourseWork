using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

static class UnityCharacterIntegration
{
    [MenuItem("Assets/Create/CharacterAsset")]
    public static void CreateYourScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<CharacterAsset>();
    }
}