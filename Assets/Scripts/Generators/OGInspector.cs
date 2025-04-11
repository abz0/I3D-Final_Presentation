using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectGenerator))]
public class OGInspector : Editor
{
    public override void OnInspectorGUI()
    {
        string infoBox1 = "Once the gameobjects have been generated, make a change in the scene to save the generated objects.";
        string infoBox2 = "If there are any child gameobjects after loading up the unity file, delete them since the current script does not have them recorded after exiting the unity file.";
        EditorGUILayout.HelpBox(infoBox1, MessageType.Info);
        EditorGUILayout.HelpBox(infoBox2, MessageType.Info);

        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);

        ObjectGenerator genDeco = (ObjectGenerator)target;

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

