using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPuppetry : MonoBehaviour
{
    [Header("Smoothing Speeds")]
    [SerializeField, Tooltip("Speed at which IK weights will transition")] 
    private float weightSmoothSpeed = 5f;
    [SerializeField, Tooltip("Speed at which IK targets will interpolate")] 
    private float positionSmoothSpeed = 5f;

    [Header("Head")]
    [SerializeField] private Transform headTarget;
    [SerializeField] private float headIKWeight = 0f;
    private float currentHeadIKWeight;
    private Vector3 currentHeadPos;

    [Header("Right Arm")]
    [SerializeField] private Transform rightElbowTarget;
    [SerializeField] private float rightElbowIKWeight = 0f;
    private float currentRightElbowIKWeight;
    private Vector3 currentRightElbowPos;

    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private float rightHandIKWeight = 0f;
    private float currentRightHandIKWeight;
    private Vector3 currentRightHandPos;

    [Header("Left Arm")]
    [SerializeField] private Transform leftElbowTarget;
    [SerializeField] private float leftElbowIKWeight = 0f;
    private float currentLeftElbowIKWeight;
    private Vector3 currentLeftElbowPos;

    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private float leftHandIKWeight = 0f;
    private float currentLeftHandIKWeight;
    private Vector3 currentLeftHandPos;

    [Header("Right Leg")]
    [SerializeField] private Transform rightKneeTarget;
    [SerializeField] private float rightKneeIKWeight = 0f;
    private float currentRightKneeIKWeight;
    private Vector3 currentRightKneePos;

    [SerializeField] private Transform rightFootTarget;
    [SerializeField] private float rightFootIKWeight = 0f;
    private float currentRightFootIKWeight;
    private Vector3 currentRightFootPos;

    [Header("Left Leg")]
    [SerializeField] private Transform leftKneeTarget;
    [SerializeField] private float leftKneeIKWeight = 0f;
    private float currentLeftKneeIKWeight;
    private Vector3 currentLeftKneePos;

    [SerializeField] private Transform leftFootTarget;
    [SerializeField] private float leftFootIKWeight = 0f;
    private float currentLeftFootIKWeight;
    private Vector3 currentLeftFootPos;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning("Animator component not found on CharacterPuppetry.");

        // initialize current weights & positions
        currentHeadIKWeight        = headIKWeight;
        currentRightElbowIKWeight  = rightElbowIKWeight;
        currentRightHandIKWeight   = rightHandIKWeight;
        currentLeftElbowIKWeight   = leftElbowIKWeight;
        currentLeftHandIKWeight    = leftHandIKWeight;
        currentRightKneeIKWeight   = rightKneeIKWeight;
        currentRightFootIKWeight   = rightFootIKWeight;
        currentLeftKneeIKWeight    = leftKneeIKWeight;
        currentLeftFootIKWeight    = leftFootIKWeight;

        if (headTarget != null) currentHeadPos = headTarget.position;
        if (rightElbowTarget != null) currentRightElbowPos = rightElbowTarget.position;
        if (rightHandTarget != null) currentRightHandPos = rightHandTarget.position;
        if (leftElbowTarget != null) currentLeftElbowPos = leftElbowTarget.position;
        if (leftHandTarget != null) currentLeftHandPos = leftHandTarget.position;
        if (rightKneeTarget != null) currentRightKneePos = rightKneeTarget.position;
        if (rightFootTarget != null) currentRightFootPos = rightFootTarget.position;
        if (leftKneeTarget != null) currentLeftKneePos = leftKneeTarget.position;
        if (leftFootTarget != null) currentLeftFootPos = leftFootTarget.position;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // Smooth weights
        currentHeadIKWeight        = Mathf.MoveTowards(currentHeadIKWeight, headIKWeight, weightSmoothSpeed * dt);
        currentRightElbowIKWeight  = Mathf.MoveTowards(currentRightElbowIKWeight, rightElbowIKWeight, weightSmoothSpeed * dt);
        currentRightHandIKWeight   = Mathf.MoveTowards(currentRightHandIKWeight, rightHandIKWeight, weightSmoothSpeed * dt);
        currentLeftElbowIKWeight   = Mathf.MoveTowards(currentLeftElbowIKWeight, leftElbowIKWeight, weightSmoothSpeed * dt);
        currentLeftHandIKWeight    = Mathf.MoveTowards(currentLeftHandIKWeight, leftHandIKWeight, weightSmoothSpeed * dt);
        currentRightKneeIKWeight   = Mathf.MoveTowards(currentRightKneeIKWeight, rightKneeIKWeight, weightSmoothSpeed * dt);
        currentRightFootIKWeight   = Mathf.MoveTowards(currentRightFootIKWeight, rightFootIKWeight, weightSmoothSpeed * dt);
        currentLeftKneeIKWeight    = Mathf.MoveTowards(currentLeftKneeIKWeight, leftKneeIKWeight, weightSmoothSpeed * dt);
        currentLeftFootIKWeight    = Mathf.MoveTowards(currentLeftFootIKWeight, leftFootIKWeight, weightSmoothSpeed * dt);

        // Smooth positions
        if (headTarget != null)
            currentHeadPos = Vector3.Lerp(currentHeadPos, headTarget.position, positionSmoothSpeed * dt);

        if (rightElbowTarget != null)
            currentRightElbowPos = Vector3.Lerp(currentRightElbowPos, rightElbowTarget.position, positionSmoothSpeed * dt);

        if (rightHandTarget != null)
            currentRightHandPos = Vector3.Lerp(currentRightHandPos, rightHandTarget.position, positionSmoothSpeed * dt);

        if (leftElbowTarget != null)
            currentLeftElbowPos = Vector3.Lerp(currentLeftElbowPos, leftElbowTarget.position, positionSmoothSpeed * dt);

        if (leftHandTarget != null)
            currentLeftHandPos = Vector3.Lerp(currentLeftHandPos, leftHandTarget.position, positionSmoothSpeed * dt);

        if (rightKneeTarget != null)
            currentRightKneePos = Vector3.Lerp(currentRightKneePos, rightKneeTarget.position, positionSmoothSpeed * dt);

        if (rightFootTarget != null)
            currentRightFootPos = Vector3.Lerp(currentRightFootPos, rightFootTarget.position, positionSmoothSpeed * dt);

        if (leftKneeTarget != null)
            currentLeftKneePos = Vector3.Lerp(currentLeftKneePos, leftKneeTarget.position, positionSmoothSpeed * dt);

        if (leftFootTarget != null)
            currentLeftFootPos = Vector3.Lerp(currentLeftFootPos, leftFootTarget.position, positionSmoothSpeed * dt);
    }

    void OnAnimatorIK(int layerIndex)
    {
        // Head
        if (headTarget != null)
            CharacterLookAtTarget(currentHeadPos, currentHeadIKWeight);

        // Right Arm
        if (rightElbowTarget != null)
            CharacterIKHintPositionToTarget(AvatarIKHint.RightElbow, currentRightElbowPos, currentRightElbowIKWeight);
        if (rightHandTarget != null)
            CharacterIKPositionToTarget(AvatarIKGoal.RightHand, currentRightHandPos, currentRightHandIKWeight);

        // Left Arm
        if (leftElbowTarget != null)
            CharacterIKHintPositionToTarget(AvatarIKHint.LeftElbow, currentLeftElbowPos, currentLeftElbowIKWeight);
        if (leftHandTarget != null)
            CharacterIKPositionToTarget(AvatarIKGoal.LeftHand, currentLeftHandPos, currentLeftHandIKWeight);

        // Right Leg
        if (rightKneeTarget != null)
            CharacterIKHintPositionToTarget(AvatarIKHint.RightKnee, currentRightKneePos, currentRightKneeIKWeight);
        if (rightFootTarget != null)
            CharacterIKPositionToTarget(AvatarIKGoal.RightFoot, currentRightFootPos, currentRightFootIKWeight);

        // Left Leg
        if (leftKneeTarget != null)
            CharacterIKHintPositionToTarget(AvatarIKHint.LeftKnee, currentLeftKneePos, currentLeftKneeIKWeight);
        if (leftFootTarget != null)
            CharacterIKPositionToTarget(AvatarIKGoal.LeftFoot, currentLeftFootPos, currentLeftFootIKWeight);
    }

    // Helpers
    private void CharacterLookAtTarget(Vector3 pos, float weight)
    {
        animator.SetLookAtWeight(weight);
        animator.SetLookAtPosition(pos);
    }

    private void CharacterIKPositionToTarget(AvatarIKGoal goal, Vector3 pos, float weight)
    {
        animator.SetIKPositionWeight(goal, weight);
        animator.SetIKPosition(goal, pos);
    }

    private void CharacterIKHintPositionToTarget(AvatarIKHint hint, Vector3 pos, float weight)
    {
        animator.SetIKHintPositionWeight(hint, weight);
        animator.SetIKHintPosition(hint, pos);
    }

    /// <summary>
    /// Puppet every joint by assigning targets and weights in one call.
    /// </summary>
    public void PuppetAll(
        Transform head, float headWeight,
        Transform rElbow, float rElbowWeight,
        Transform rHand, float rHandWeight,
        Transform lElbow, float lElbowWeight,
        Transform lHand, float lHandWeight,
        Transform rKnee, float rKneeWeight,
        Transform rFoot, float rFootWeight,
        Transform lKnee, float lKneeWeight,
        Transform lFoot, float lFootWeight)
    {
        // Targets
        headTarget = head;
        rightElbowTarget = rElbow;
        rightHandTarget = rHand;
        leftElbowTarget = lElbow;
        leftHandTarget = lHand;
        rightKneeTarget = rKnee;
        rightFootTarget = rFoot;
        leftKneeTarget = lKnee;
        leftFootTarget = lFoot;

        // Weights
        headIKWeight = headWeight;
        rightElbowIKWeight = rElbowWeight;
        rightHandIKWeight = rHandWeight;
        leftElbowIKWeight = lElbowWeight;
        leftHandIKWeight = lHandWeight;
        rightKneeIKWeight = rKneeWeight;
        rightFootIKWeight = rFootWeight;
        leftKneeIKWeight = lKneeWeight;
        leftFootIKWeight = lFootWeight;
    }

    /// <summary>
    /// Tell this character to IK–aim its RightHand at target with a full weight.
    /// </summary>
    public void GrabAt(Transform target)
    {
        rightHandTarget = target;
        rightHandIKWeight = 1f;
    }

    /// <summary>
    /// Release the hand back to its default animation.
    /// </summary>
    public void ReleaseHand()
    {
        rightHandIKWeight = 0f;
    }

    /// <summary>
    /// Smoothly ramp *all* IK weights back to zero,
    /// but leave the last-used targets in place for the duration of the fade.
    /// </summary>
    public void ReleaseAllSmooth()
    {
        PuppetAll(
            headTarget,         0f,
            rightElbowTarget,   0f,
            rightHandTarget,    0f,
            leftElbowTarget,    0f,
            leftHandTarget,     0f,
            rightKneeTarget,    0f,
            rightFootTarget,    0f,
            leftKneeTarget,     0f,
            leftFootTarget,     0f
        );
    }

    // Your existing public methods (GrabAt, ReleaseHand, PuppetAll, ReleaseAllSmooth) remain unchanged
}
