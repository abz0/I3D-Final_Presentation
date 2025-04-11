using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BuildPlanks : MonoBehaviour
{
    public GameObject anchorObject;
    public GameObject plankObject;

    [Header("Planks")]
    [SerializeField] private int amount = 3;
    [SerializeField] private float buildGap = 0f;
    [SerializeField] private PKlocalScale byLocal;

    [Header("Build")]
    [SerializeField] private bool areAnchorsVisible = false;
    [SerializeField] private bool usesGravity = false;
    [SerializeField] private int fallBy = 10;
    [SerializeField] private float duration = 1f;

    private List<Transform> bridgeSections = new List<Transform>();
    private enum BSIndex { anchor, plank };
    private int bpIndex = 0;

    private float timer;
    public enum PKlocalScale { x, y, z };

    void Update()
    {
        //if (usesGravity)
        //{
        //    if (bpIndex < fallBy)
        //    {
        //        timer -= Time.deltaTime;

        //        if (timer <= 0)
        //        {
        //            Transform plankSection = transform.Find("Planks");
        //            for (int i = bpIndex; i < plankSection.childCount; i += fallBy)
        //            {
        //                Rigidbody rb = plankSection.GetChild(i).GetComponent<Rigidbody>();

        //                rb.useGravity = true;
        //                rb.isKinematic = false;
        //            }

        //            duration += Time.deltaTime;
        //            timer = duration;
        //            bpIndex++;
        //        }
        //    }
        //}
    }

    //gets the addition needed for the next build
    private Vector3 AddBuildObjectPosition(bool isAnchor)
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

        return position + new Vector3(buildGap, 0, 0);
    }

    private void InstantiateBridgeSections()
    {
        GameObject anchorSection = new GameObject("Anchors");
        GameObject plankSection = new GameObject("Planks");

        anchorSection.transform.parent = plankSection.transform.parent = transform;

        bridgeSections.Add(anchorSection.transform);
        bridgeSections.Add(plankSection.transform);
    }

    private void InstantiateBridgeParts()
    {
        Vector3 buildPosition = transform.position;
        for (int i = 0; i < amount; i++)
        {
            GameObject plank = Instantiate(plankObject, bridgeSections[(int)BSIndex.plank]); //sort the plankObjects into the Planks gameobject

            if (i == 0) buildPosition += AddBuildObjectPosition(true);
            else buildPosition += AddBuildObjectPosition(false);

            plank.transform.position = buildPosition;
        }

        for (int i = 0; i < 2; i++)
        {
            GameObject anchor = Instantiate(anchorObject, bridgeSections[(int)BSIndex.anchor]);

            if (i == 0) anchor.transform.position = transform.position;
            else anchor.transform.position = buildPosition + AddBuildObjectPosition(true);
        }
    }

    //adds and sets the rigidbodies to the bridge parts, where it is required for the joint class
    private void AddRigidBody() 
    {
        foreach (Transform anchor in bridgeSections[(int)(BSIndex.anchor)])
        {
            Rigidbody rb = anchor.gameObject.GetComponent<Rigidbody>();
            if (rb == null) rb = anchor.gameObject.AddComponent<Rigidbody>();

            rb.useGravity = false;
            rb.isKinematic = true;
        }

        foreach (Transform plank in bridgeSections[(int)(BSIndex.plank)])
        {
            Rigidbody rb = plank.gameObject.GetComponent<Rigidbody>();
            if (rb == null) rb = plank.gameObject.AddComponent<Rigidbody>();

            //rb.useGravity = false;
            //rb.isKinematic = true;
            if (usesGravity)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
            else
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }

    //connects the bridge parts together using hingejoints
    private void AddJoints()
    {
        Transform anchorSection = bridgeSections[(int)(BSIndex.anchor)];
        Transform plankSection = bridgeSections[(int)(BSIndex.plank)];
        for (int i = 0; i < plankSection.childCount + 1; i++) //this includes the plankSection children and the last anchor
        {
            GameObject bridgePart;
            GameObject prevBridgePart;
            HingeJoint hj;
            Rigidbody rb;
            if (i < plankSection.childCount)
            {
                bridgePart = plankSection.GetChild(i).gameObject;

                if (i == 0) prevBridgePart = anchorSection.GetChild(0).gameObject;
                else prevBridgePart = plankSection.GetChild(i - 1).gameObject;
            }
            else
            {
                bridgePart = anchorSection.GetChild(1).gameObject;

                prevBridgePart = plankSection.GetChild(plankSection.childCount - 1).gameObject;
            }

            hj = bridgePart.GetComponent<HingeJoint>();
            if (hj == null) hj = bridgePart.AddComponent<HingeJoint>();

            rb = prevBridgePart.GetComponent<Rigidbody>();

            hj.connectedBody = rb;
            hj.anchor = new Vector3(0, 0, 0);
            hj.axis = new Vector3(0, 0, 1);
        }
    }

    private void ShowAnchors()
    {
        foreach (Transform anchor in bridgeSections[(int)(BSIndex.anchor)])
        {
            MeshRenderer meshRenderer;
            if (meshRenderer = anchor.gameObject.GetComponent<MeshRenderer>())
            {
                if (areAnchorsVisible) meshRenderer.enabled = true;
                else meshRenderer.enabled = false;
            }
        }
    }

    public void BuildBridge()
    {
        InstantiateBridgeSections();
        InstantiateBridgeParts();
        AddRigidBody();
        AddJoints();
        ShowAnchors();

        timer = duration;
    }

    public void ClearBridge()
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

        bridgeSections.Clear();
    }
}
