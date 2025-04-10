using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenerateDecorations))]
public class GenDecoInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GenerateDecorations genDeco = (GenerateDecorations)target;
        if (GUILayout.Button("Generate Decorations"))
        {
            genDeco.GenerateObjects();
        }
    }
}

