using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPuppetry : MonoBehaviour
{
    [Header("Smoothing")]
    [SerializeField, Tooltip("Speed at which IK weights will smoothly transition")] private float weightSmoothSpeed = 5f;

    [Header("Head")]
    [SerializeField] private Transform headTarget;
    [SerializeField] private float headIKWeight = 0f;
    private float currentHeadIKWeight;

    [Header("Right Arm")]
    [SerializeField] private Transform rightElbowTarget;
    [SerializeField] private float rightElbowIKWeight = 0f;
    private float currentRightElbowIKWeight;
    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private float rightHandIKWeight = 0f;
    private float currentRightHandIKWeight;

    [Header("Left Arm")]
    [SerializeField] private Transform leftElbowTarget;
    [SerializeField] private float leftElbowIKWeight = 0f;
    private float currentLeftElbowIKWeight;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private float leftHandIKWeight = 0f;
    private float currentLeftHandIKWeight;

    [Header("Right Leg")]
    [SerializeField] private Transform rightKneeTarget;
    [SerializeField] private float rightKneeIKWeight = 0f;
    private float currentRightKneeIKWeight;
    [SerializeField] private Transform rightFootTarget;
    [SerializeField] private float rightFootIKWeight = 0f;
    private float currentRightFootIKWeight;

    [Header("Left Leg")]
    [SerializeField] private Transform leftKneeTarget;
    [SerializeField] private float leftKneeIKWeight = 0f;
    private float currentLeftKneeIKWeight;
    [SerializeField] private Transform leftFootTarget;
    [SerializeField] private float leftFootIKWeight = 0f;
    private float currentLeftFootIKWeight;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning("Animator component not found on CharacterPuppetry.");

        // Initialize current weights to starting target weights
        currentHeadIKWeight = headIKWeight;
        currentRightElbowIKWeight = rightElbowIKWeight;
        currentRightHandIKWeight = rightHandIKWeight;
        currentLeftElbowIKWeight = leftElbowIKWeight;
        currentLeftHandIKWeight = leftHandIKWeight;
        currentRightKneeIKWeight = rightKneeIKWeight;
        currentRightFootIKWeight = rightFootIKWeight;
        currentLeftKneeIKWeight = leftKneeIKWeight;
        currentLeftFootIKWeight = leftFootIKWeight;
    }

    void Update()
    {
        // Smoothly update current weights toward target weights
        currentHeadIKWeight = Mathf.MoveTowards(currentHeadIKWeight, headIKWeight, weightSmoothSpeed * Time.deltaTime);
        currentRightElbowIKWeight = Mathf.MoveTowards(currentRightElbowIKWeight, rightElbowIKWeight, weightSmoothSpeed * Time.deltaTime);
        currentRightHandIKWeight = Mathf.MoveTowards(currentRightHandIKWeight, rightHandIKWeight, weightSmoothSpeed * Time.deltaTime);
        currentLeftElbowIKWeight = Mathf.MoveTowards(currentLeftElbowIKWeight, leftElbowIKWeight, weightSmoothSpeed * Time.deltaTime);
        currentLeftHandIKWeight = Mathf.MoveTowards(currentLeftHandIKWeight, leftHandIKWeight, weightSmoothSpeed * Time.deltaTime);
        currentRightKneeIKWeight = Mathf.MoveTowards(currentRightKneeIKWeight, rightKneeIKWeight, weightSmoothSpeed * Time.deltaTime);
        currentRightFootIKWeight = Mathf.MoveTowards(currentRightFootIKWeight, rightFootIKWeight, weightSmoothSpeed * Time.deltaTime);
        currentLeftKneeIKWeight = Mathf.MoveTowards(currentLeftKneeIKWeight, leftKneeIKWeight, weightSmoothSpeed * Time.deltaTime);
        currentLeftFootIKWeight = Mathf.MoveTowards(currentLeftFootIKWeight, leftFootIKWeight, weightSmoothSpeed * Time.deltaTime);
    }

    void OnAnimatorIK(int layerIndex)
    {
        // Head
        if (headTarget != null)
            CharacterLookAtTarget(headTarget, currentHeadIKWeight);

        // Right Arm
        if (rightElbowTarget != null)
            CharacterIKHintPositionToTarget(AvatarIKHint.RightElbow, rightElbowTarget, currentRightElbowIKWeight);
        if (rightHandTarget != null)
            CharacterIKPositionToTarget(AvatarIKGoal.RightHand, rightHandTarget, currentRightHandIKWeight);

        // Left Arm
        if (leftElbowTarget != null)
            CharacterIKHintPositionToTarget(AvatarIKHint.LeftElbow, leftElbowTarget, currentLeftElbowIKWeight);
        if (leftHandTarget != null)
            CharacterIKPositionToTarget(AvatarIKGoal.LeftHand, leftHandTarget, currentLeftHandIKWeight);

        // Right Leg
        if (rightKneeTarget != null)
            CharacterIKHintPositionToTarget(AvatarIKHint.RightKnee, rightKneeTarget, currentRightKneeIKWeight);
        if (rightFootTarget != null)
            CharacterIKPositionToTarget(AvatarIKGoal.RightFoot, rightFootTarget, currentRightFootIKWeight);

        // Left Leg
        if (leftKneeTarget != null)
            CharacterIKHintPositionToTarget(AvatarIKHint.LeftKnee, leftKneeTarget, currentLeftKneeIKWeight);
        if (leftFootTarget != null)
            CharacterIKPositionToTarget(AvatarIKGoal.LeftFoot, leftFootTarget, currentLeftFootIKWeight);
    }

    /// <summary>
    /// Tell this character to IK–aim its RightHand at `target` with a full weight.
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

    // Helper to set look at
    private void CharacterLookAtTarget(Transform target, float weight)
    {
        animator.SetLookAtWeight(weight);
        animator.SetLookAtPosition(target.position);
    }

    // Helper to set IK position
    private void CharacterIKPositionToTarget(AvatarIKGoal goal, Transform target, float weight)
    {
        animator.SetIKPositionWeight(goal, weight);
        animator.SetIKPosition(goal, target.position);
    }

    // Helper to set IK hint
    private void CharacterIKHintPositionToTarget(AvatarIKHint hint, Transform target, float weight)
    {
        animator.SetIKHintPositionWeight(hint, weight);
        animator.SetIKHintPosition(hint, target.position);
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

}
