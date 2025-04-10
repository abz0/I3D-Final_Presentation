using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDecorations : MonoBehaviour
{
    [SerializeField] private Vector2 spawnArea = new Vector2(1, 1);
    [SerializeField] private List<GenDecoObject> objects = new List<GenDecoObject>();
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
        Vector3 originPosition = transform.position;
        Vector3 realSpawnArea = originPosition + new Vector3(spawnArea.x, originPosition.y, spawnArea.y) / 2;

        float randX = Random.Range(-realSpawnArea.x, realSpawnArea.x);
        float randZ = Random.Range(-realSpawnArea.z, realSpawnArea.z);

        return new Vector3(randX, originPosition.y, randZ); ;
    }

    private bool ValidSpawnPosition(GenDecoObject go, Vector3 position)
    {
        Vector3 capsuleEndCast = new Vector3(position.x, position.y + go.height, position.z);
        
        if (Physics.CapsuleCastAll(position, capsuleEndCast, go.radius, Vector3.down).Length > 0) return false;

        return true;
    }

    private void InstantiateObject(GameObject go, Vector3 position)
    {
        
        GameObject obj = Instantiate(go, transform);
        obj.transform.position = position;
        obj.AddComponent<CapsuleCollider>();

        existingObjects.Add(obj);
    }

    private bool SpawnObject(GenDecoObject go)
    {
        Vector3 spawnPosition = GetSpawnPosition();

        bool canSpawn = false;
        for (int i = 0;  i < 5; i++)
        {
            if (ValidSpawnPosition(go, spawnPosition))
            {
                canSpawn = true;
                break;
            }

            spawnPosition = GetSpawnPosition();
        }

        if (canSpawn)
        {
            InstantiateObject(go.obj, spawnPosition);
            
            return true;
        }

        return false;
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
            for (int j = 0; j < 5; j++)
            {
                if (SpawnObject(objects[Random.Range(0, objects.Count)])) break;
            }
        }

        Debug.Log(existingObjects.Count + " Objects generated");
    }
}
