using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenerateDecorations))]
public class GenDecoInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GenerateDecorations genDeco = (GenerateDecorations)target;
        if (GUILayout.Button("Generate Decorations"))
        {
            genDeco.ClearExistingObjects();
            genDeco.GenerateObjects();
        }
    }
}

