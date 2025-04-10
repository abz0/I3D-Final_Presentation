using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildPlanks))]
public class BPInspector : Editor
{
    public override void OnInspectorGUI()
    {
        string infoBox1 = "If there are any child gameobjects after loading up the unity file, delete them since the current script does not have them recorded after exiting the unity file.";
        EditorGUILayout.HelpBox(infoBox1, MessageType.Warning);

        string infoBox2 = "Once the gameobjects have been generated, make a change in the scene to save the generated objects";
        EditorGUILayout.HelpBox(infoBox2, MessageType.Warning);

        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);

        BuildPlanks builder = (BuildPlanks)target;

        if (GUILayout.Button("Build Bridge"))
        {
            builder.ClearBridge();
            builder.BuildBridge();
        }

        if (GUILayout.Button("Clear Bridge"))
        {
            builder.ClearBridge();
        }
    }
}
