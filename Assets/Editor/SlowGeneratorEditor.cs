using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SlowGenerator))]
public class SlowGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SlowGenerator slowGenerator = (SlowGenerator) target;
        if (GUILayout.Button("Generate Level"))
        {
            slowGenerator.GenerateLevel();
        }
        
        if (GUILayout.Button("Clear Level"))
        {
            slowGenerator.ResetLevel();
        }
    }
}
