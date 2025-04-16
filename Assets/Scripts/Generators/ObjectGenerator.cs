using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private Vector2 spawnArea = new Vector2(1, 1);
    [Tooltip("Sort the objects in a specific order as the generation goes through the list one by one for each spawn")]
    [SerializeField] private List<OGObject> objects = new List<OGObject>();
    [SerializeField] private int randomAmount = 1;

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

    private bool ValidSpawnPosition(OGObject go, Vector3 position)
    {
        Vector3 capsuleEndCast = new Vector3(position.x, position.y + go.height, position.z);

        if (Physics.CapsuleCastAll(position, capsuleEndCast, go.radius, Vector3.down).Length > 1) return false;

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
        if (!obj.GetComponent<CapsuleCollider>()) obj.AddComponent<CapsuleCollider>();

        existingObjects.Add(obj);
    }

    private bool SpawnObject(OGObject go)
    {
        Vector3 spawnPosition = GetSpawnPosition();

        if (ValidSpawnPosition(go, spawnPosition))
        {
            InstantiateObject(go.obj, spawnPosition);

            return true;
        }

        return false;
    }

    // clears the existing objects on scene
    public void ClearExistingObjects()
    {
        List<GameObject> existingSections = new List<GameObject>();
        foreach (Transform sections in transform)
        {
            existingSections.Add(sections.gameObject);
        }

        foreach (GameObject sections in existingSections)
        {
            DestroyImmediate(sections);
        }

        objectTypes.Clear();
        existingObjects.Clear();
    }

    private bool Generate(OGObject obj, int attempts)
    {
        for (int i = 0; i < attempts; i++)
        {
            if (SpawnObject(obj)) return true;
        }

        return false;
    }

    public void GenerateObjects()
    {
        foreach (OGObject go in objects)
        {
            for (int i = 0; i < go.amount; i++)
            {
                Generate(go, 5);
            }
        }

        Debug.Log(existingObjects.Count + " Objects generated");
    }

    public void GenerateRandomObjects()
    {
        int spawnAmount = 0;
        int randomSpawnAttempts = 0; //attempts at spawning if a spawn while loop fails
        while (spawnAmount < randomAmount && randomSpawnAttempts < 3)
        {
            bool didSpawn = false; //if an object have already been spawned into the scene
            foreach (OGObject obj in objects)
            {
                float chance = Random.Range(0f, 1f);
                if (chance <= obj.randomChance)
                {
                    if (Generate(obj, 5)) //when a new object is spawned
                    {
                        spawnAmount++;

                        if (!didSpawn) didSpawn = true;
                    }
                }

                if (spawnAmount >= randomAmount) break; //breaks out of the foreach loop if the spawnAmount reaches the randomAmount
            }

            if (!didSpawn) randomSpawnAttempts++;
        }

        Debug.Log(existingObjects.Count + " Objects generated");
    }

    //adds the measurements of the cast of the object
    private void SetObjectDefaultCast(OGObject decoObject)
    {
        GameObject temp = Instantiate(decoObject.obj);
        CapsuleCollider collider = temp.GetComponent<CapsuleCollider>();
        if (!collider) collider = temp.AddComponent<CapsuleCollider>();

        decoObject.SetRadius(collider.radius);
        decoObject.SetHeight(collider.height);

        DestroyImmediate(temp);
    }

    public void SetAllObjectDefaultCast()
    {
        foreach(OGObject decoObject in objects)
        {
            SetObjectDefaultCast(decoObject);
        }
    }
}
