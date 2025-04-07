using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPlanks : MonoBehaviour
{
    public GameObject anchorObject;
    public GameObject plankObject;

    [Header("Planks")]
    public int amount = 3;
    public float width = 1f;
    public float height = 1f;
    public float depth = 1f;
    public float gap = 0.1f;

    private Vector3 buildLocation;
    private Rigidbody connectingObject;

    void Start()
    {
        buildLocation = anchorObject.transform.position + new Vector3(anchorObject.transform.localScale.x, 0, 0);
        connectingObject = anchorObject.GetComponent<Rigidbody>();

        CreateBridge();

        GameObject endAnchor = Instantiate(anchorObject, buildLocation, Quaternion.identity);
        endAnchor.AddComponent<HingeJoint>();
        endAnchor.GetComponent<HingeJoint>().connectedBody = connectingObject;
        endAnchor.GetComponent<HingeJoint>().axis = new Vector3(0, 0, 1);
        endAnchor.GetComponent<BuildPlanks>().enabled = false;
    }

    public void CreateBridge()
    {
        for (int i = 0; i < amount; i++)
        {
            CreatePlank();
        }
    }

    public void CreatePlank()
    {
        GameObject plank = Instantiate(plankObject, buildLocation, Quaternion.identity);

        plank.transform.localScale = new Vector3(width, height, depth);

        plank.GetComponent<HingeJoint>().connectedBody = connectingObject;

        buildLocation += new Vector3(plank.transform.localScale.x + gap, 0, 0);
        connectingObject = plank.GetComponent<Rigidbody>();
    }
}
