using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPlanks : MonoBehaviour
{
    public GameObject anchorObject;
    public GameObject plankObject;

    [Header("Planks")]
    public int amount = 3;
    public float buildGap = 0f;

    private List<GameObject> bridgeParts = new List<GameObject>();

    void Start()
    {
        CreateBridgeParts();
        SetBridgePartLocations();
        AddRigidBody();
        AddJoints();
    }

    public void CreateBridgeParts()
    {
        bridgeParts.Add(Instantiate(anchorObject, transform));

        for (int i = 0; i < amount; i++)
        {
            bridgeParts.Add(Instantiate(plankObject, transform));
        }

        bridgeParts.Add(Instantiate(anchorObject, transform));
    }

    public void SetBridgePartLocations()
    {
        Vector3 newLocation = transform.position;

        bridgeParts[0].transform.position = newLocation;

        newLocation += new Vector3(anchorObject.transform.localScale.x + plankObject.transform.localScale.x, 0, 0) / 2;
        newLocation += new Vector3(buildGap, 0, 0);

        for (int i = 1; i < bridgeParts.Count - 1; i++)
        {
            bridgeParts[i].transform.position = newLocation;

            if (i != bridgeParts.Count - 2) 
            {
                newLocation += new Vector3(plankObject.transform.localScale.x, 0, 0);
            }
            else 
            {
                newLocation += new Vector3(anchorObject.transform.localScale.x + plankObject.transform.localScale.x, 0, 0) / 2;
            }

            newLocation += new Vector3(buildGap, 0, 0);
        }

        bridgeParts[bridgeParts.Count - 1].transform.position = newLocation;
    }

    public void AddRigidBody() 
    {
        for (int i = 0; i < bridgeParts.Count; i++)
        {
            GameObject bridgePart = bridgeParts[i];

            Rigidbody rb = bridgePart.GetComponent<Rigidbody>();
            if (rb == null) rb = bridgePart.AddComponent<Rigidbody>();

            if (i == 0 || i == bridgeParts.Count - 1)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
            else
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }
    }

    public void AddJoints()
    {
        for (int i = 1; i < bridgeParts.Count; i++)
        {
            GameObject bridgePart = bridgeParts[i];

            FixedJoint hj = bridgePart.GetComponent<FixedJoint>();
            if (hj == null) hj = bridgePart.AddComponent<FixedJoint>();

            GameObject prevBridgePart = bridgeParts[i - 1];
            Rigidbody rb = prevBridgePart.GetComponent<Rigidbody>();

            hj.connectedBody = rb;
            hj.anchor = new Vector3(0, 0, 0);
            hj.axis = new Vector3(0, 0, 1);
        }
    }
}
