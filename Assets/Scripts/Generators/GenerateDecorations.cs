using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDecorations : MonoBehaviour
{
    [SerializeField] private Vector2 spawnArea = new Vector2(1, 1);
    [SerializeField] private List<GameObject> decorations = new List<GameObject>();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, 0, spawnArea.y));
    }

    private void SpawnObject(GameObject go)
    {
        Instantiate(go, transform);
    }

    public void GenerateObjects()
    {
        SpawnObject(decorations[0]);
    }
}
