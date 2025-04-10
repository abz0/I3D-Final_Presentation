using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDecorations : MonoBehaviour
{
    [SerializeField] private Vector2 spawnArea = new Vector2(1, 1);
    [SerializeField] private List<GameObject> objects = new List<GameObject>();
    [SerializeField] private int amount = 0;

    private List<GameObject> existingObjects = new List<GameObject>();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, 0, spawnArea.y));
    }

    private void SpawnObject(GameObject go)
    {
        existingObjects.Add(Instantiate(go, transform));
    }

    public void ClearExistingObjects()
    {
        foreach(GameObject go in existingObjects)
        {
            DestroyImmediate(go);
        }

        existingObjects.Clear();
    }

    public void GenerateObjects()
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnObject(objects[Random.Range(0, objects.Count)]);
        }
    }
}
