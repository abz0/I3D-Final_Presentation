using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenerateDecorations))]
public class GenDecoInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);

        GenerateDecorations genDeco = (GenerateDecorations)target;

        if (GUILayout.Button("Set All Object Default Cast"))
        {
            genDeco.SetAllObjectDefaultCast();
        }

        if (GUILayout.Button("Generate Objects"))
        {
            genDeco.ClearExistingObjects();
            genDeco.GenerateObjects();
        }

        if (GUILayout.Button("Generate Random Objects"))
        {
            genDeco.ClearExistingObjects();
            genDeco.GenerateRandomObjects();
        }

        if (GUILayout.Button("Clear Objects"))
        {
            genDeco.ClearExistingObjects();
        }
    }
}

