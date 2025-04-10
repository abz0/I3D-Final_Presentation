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

        if (GUILayout.Button("Generate Decorations"))
        {
            genDeco.ClearExistingObjects();
            genDeco.GenerateObjects();
        }

        if (GUILayout.Button("Clear Decorations"))
        {
            genDeco.ClearExistingObjects();
        }
    }
}

