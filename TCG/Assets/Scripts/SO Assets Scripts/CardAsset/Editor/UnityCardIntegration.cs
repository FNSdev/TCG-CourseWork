using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

static class UnityCardIntegration
{
    [MenuItem("Assets/Create/CardAsset")]
    public static void CreateYourScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<CardAsset>();
    }
}