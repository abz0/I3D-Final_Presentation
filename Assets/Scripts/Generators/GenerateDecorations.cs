using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDecorations : MonoBehaviour
{
    [SerializeField] private Vector2 spawnArea = new Vector2(1, 1);
    [SerializeField] private List<GameObject> objects = new List<GameObject>();
    [SerializeField] private int amount = 0;

    private List<GameObject> existingObjects = new List<GameObject>();

    // Unity // // // //
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, 0, spawnArea.y));
    }

    // Objects // // // //
    private Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition = transform.position;
        Vector3 realSpawnArea = spawnPosition + new Vector3(spawnArea.x, spawnPosition.y, spawnArea.y) / 2;

        int randX = Random.Range((int)-realSpawnArea.x, (int)(realSpawnArea.x + 1));
        int randZ = Random.Range((int)-realSpawnArea.z, (int)(realSpawnArea.z + 1));

        spawnPosition = new Vector3(randX, spawnPosition.y, randZ);

        return spawnPosition;
    }

    private void SpawnObject(GameObject go)
    {
        GameObject obj = Instantiate(go, transform);
        obj.transform.position = GetSpawnPosition();

        existingObjects.Add(obj);
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
