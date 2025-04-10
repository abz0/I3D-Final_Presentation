using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDecorations : MonoBehaviour
{
    [SerializeField] private Vector2 spawnArea = new Vector2(1, 1);
    [SerializeField] private List<GenDecoObject> objects = new List<GenDecoObject>();
    [SerializeField] private int randomAmount = 0;

    private List<GameObject> existingObjects = new List<GameObject>();
    private List<Transform> objectTypes = new List<Transform>();

    // Unity // // // //
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, 0, spawnArea.y));
    }

    // Objects // // // //
    private Vector3 GetSpawnPosition()
    {
        Vector3 origin = transform.position;
        Vector2 realSpawnArea = spawnArea / 2;

        float randX = Random.Range(origin.x - realSpawnArea.x, origin.x + realSpawnArea.x);
        float randZ = Random.Range(origin.z - realSpawnArea.y, origin.z + realSpawnArea.y);

        return new Vector3(randX, origin.y, randZ);
    }

    private bool ValidSpawnPosition(GenDecoObject go, Vector3 position)
    {
        Vector3 capsuleEndCast = new Vector3(position.x, position.y + go.height, position.z);
        
        if (Physics.CapsuleCastAll(position, capsuleEndCast, go.radius, Vector3.down).Length > 0) return false;

        return true;
    }

    private Transform GetObjectType(string name)
    {
        foreach (Transform t in objectTypes)
        {
            if (t.name == name) return t;
        }


        GameObject newObjectType = new GameObject(name);
        newObjectType.transform.parent = transform;

        objectTypes.Add(newObjectType.transform);

        return objectTypes[objectTypes.Count - 1];
    }

    private void InstantiateObject(GameObject go, Vector3 position)
    {

        GameObject obj = Instantiate(go, GetObjectType(go.name));
        obj.transform.position = position;
        obj.AddComponent<CapsuleCollider>();

        existingObjects.Add(obj);
    }

    private bool SpawnObject(GenDecoObject go)
    {
        Vector3 spawnPosition = GetSpawnPosition();

        for (int i = 0;  i < 5; i++)
        {
            if (ValidSpawnPosition(go, spawnPosition))
            {
                InstantiateObject(go.obj, spawnPosition);
                
                return true;
            }

            spawnPosition = GetSpawnPosition();
        }

        return false;
    }

    public void ClearExistingObjects()
    {
        foreach (GameObject go in existingObjects)
        {
            DestroyImmediate(go);
        }

        foreach (Transform t in objectTypes)
        {
            DestroyImmediate(t.gameObject);
        }

        objectTypes.Clear();
        existingObjects.Clear();
    }

    private void Generate(GenDecoObject obj, int attempts)
    {
        for (int i = 0; i < attempts; i++)
        {
            if (SpawnObject(obj)) break;
        }
    }

    public void GenerateObjects()
    {
        foreach (GenDecoObject go in objects)
        {
            for (int i = 0; i < go.amount; i++)
            {
                Generate(go, 5);
            }
        }
    }

    public void GenerateRandomObjects()
    {
        for (int i = 0; i < randomAmount; i++)
        {
            Generate(objects[Random.Range(0, objects.Count)], 5);
        }
    }
}
