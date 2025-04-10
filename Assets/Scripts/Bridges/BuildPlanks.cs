using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BuildPlanks : MonoBehaviour
{
    public GameObject anchorObject;
    public GameObject plankObject;

    [Header("Planks")]
    public int amount = 3;
    public float buildGap = 0f;
    public PKlocalScale byLocal;

    [Header("Build")]
    public bool usesGravity = false;
    public float duration = 1f;

    private List<GameObject> bridgeParts = new List<GameObject>();
    private List<Transform> bridgeSections = new List<Transform>();
    private int bpIndex = 1;

    private float timer;
    public enum PKlocalScale { x, y, z };

    void Update()
    {
        if (usesGravity)
        {
            if (bpIndex < bridgeParts.Count - 2)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    Rigidbody rb = bridgeParts[bpIndex].GetComponent<Rigidbody>();

                    rb.useGravity = true;
                    rb.isKinematic = false;

                    duration += Time.deltaTime;
                    timer = duration;
                    bpIndex++;
                }
            }
        }
    }

    private void CreateBridgeSections()
    {
        GameObject anchorSection = new GameObject("Anchors");
        GameObject plankSection = new GameObject("Planks");

        anchorSection.transform.parent = plankSection.transform.parent = transform;

        bridgeSections.Add(anchorSection.transform);
        bridgeSections.Add(plankSection.transform);
    }

    private void CreateBridgeParts()
    {
        bridgeParts.Add(Instantiate(anchorObject, bridgeSections[0]));

        for (int i = 0; i < amount; i++)
        {
            bridgeParts.Add(Instantiate(plankObject, bridgeSections[1]));
        }

        bridgeParts.Add(Instantiate(anchorObject, bridgeSections[0]));
    }

    private void SetBridgePartLocations()
    {
        Vector3 newLocation = transform.position;

        bridgeParts[0].transform.position = newLocation;

        newLocation += GetObjectPosition(true);

        newLocation += new Vector3(buildGap, 0, 0);

        for (int i = 1; i < bridgeParts.Count - 1; i++)
        {
            bridgeParts[i].transform.position = newLocation;

            if (i != bridgeParts.Count - 2)
            {
                newLocation += GetObjectPosition(false);
            }
            else
            {
                newLocation += GetObjectPosition(true);
            }

            newLocation += new Vector3(buildGap, 0, 0);
        }

        bridgeParts[bridgeParts.Count - 1].transform.position = newLocation;
    }

    private Vector3 GetObjectPosition(bool isAnchor)
    {
        Vector3 position;
        
        switch (byLocal)
        {
            case PKlocalScale.x:
                if (isAnchor) position = new Vector3(anchorObject.transform.localScale.x + plankObject.transform.localScale.x, 0, 0) / 2;
                else position = new Vector3(plankObject.transform.localScale.x, 0, 0);
                break;
            case PKlocalScale.y:
                if (isAnchor) position = new Vector3(anchorObject.transform.localScale.y + plankObject.transform.localScale.y, 0, 0) / 2;
                else position = new Vector3(plankObject.transform.localScale.y, 0, 0);
                break;
            case PKlocalScale.z:
                if (isAnchor) position = new Vector3(anchorObject.transform.localScale.z + plankObject.transform.localScale.z, 0, 0) / 2;
                else position = new Vector3(plankObject.transform.localScale.z, 0, 0);
                break;
            default:
                position = Vector3.zero; 
                break;
        }

        return position;
    }

    private void AddRigidBody() 
    {
        for (int i = 0; i < bridgeParts.Count; i++)
        {
            GameObject bridgePart = bridgeParts[i];

            Rigidbody rb = bridgePart.GetComponent<Rigidbody>();
            if (rb == null) rb = bridgePart.AddComponent<Rigidbody>();

            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    private void AddJoints()
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

    public void BuildBridge()
    {
        CreateBridgeSections();
        CreateBridgeParts();
        SetBridgePartLocations();
        AddRigidBody();
        AddJoints();

        timer = duration;
    }

    public void ClearBridge()
    {
        //foreach (GameObject bridgePart in bridgeParts)
        //{
        //    DestroyImmediate(bridgePart);
        //}

        foreach (Transform bridgeSection in bridgeSections)
        {
            DestroyImmediate(bridgeSection.gameObject);
        }

        bridgeParts.Clear();
        bridgeSections.Clear();
    }
}
