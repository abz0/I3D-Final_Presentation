using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPuppetry : MonoBehaviour
{
    [Header("Head")]
    [SerializeField] private Transform headTarget;
    [SerializeField] private float headIKWeight = 0f;

    [Header("Right Arm")]
    [SerializeField] private Transform rightElbowTarget;
    [SerializeField] private float rightElbowIKWeight = 0f;
    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private float rightHandIKWeight = 0f;

    [Header("Left Arm")]
    [SerializeField] private Transform leftElbowTarget;
    [SerializeField] private float leftElbowIKWeight = 0f;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private float leftHandIKWeight = 0f;

    [Header("Right Leg")]
    [SerializeField] private Transform rightKneeTarget;
    [SerializeField] private float rightKneeIKWeight = 0f;
    [SerializeField] private Transform rightFootTarget;
    [SerializeField] private float rightFootIKWeight = 0f;

    [Header("Left Leg")]
    [SerializeField] private Transform leftKneeTarget;
    [SerializeField] private float leftKneeIKWeight = 0f;
    [SerializeField] private Transform leftFootTarget;
    [SerializeField] private float leftFootIKWeight = 0f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null) Debug.LogWarning("The Animator Component is not found");
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (headTarget) CharacterLookAtTarget(headTarget, headIKWeight);
        else Debug.LogWarning("HeadTarget Transformation is null");

        //Right Arm
        if (rightElbowTarget) CharacterIKHintPositionToTarget(AvatarIKHint.RightElbow, rightElbowTarget, rightElbowIKWeight);
        else Debug.LogWarning("RightElbowTarget Transformation is null");
        if (rightHandTarget) CharacterIKPositionToTarget(AvatarIKGoal.RightHand, rightHandTarget, rightHandIKWeight);
        else Debug.LogWarning("RightHandTarget Transformation is null");

        //Left Arm
        if (leftElbowTarget) CharacterIKHintPositionToTarget(AvatarIKHint.LeftElbow, leftElbowTarget, leftElbowIKWeight);
        else Debug.LogWarning("LeftElbowTarget Transformation is null");
        if (leftHandTarget) CharacterIKPositionToTarget(AvatarIKGoal.LeftHand, leftHandTarget, leftHandIKWeight);
        else Debug.LogWarning("LeftHandTarget Transformation is null");

        //Right Leg
        if (rightKneeTarget) CharacterIKHintPositionToTarget(AvatarIKHint.RightKnee, rightKneeTarget, rightKneeIKWeight);
        else Debug.LogWarning("RightKneeTarget Transformation is null");
        if (rightFootTarget) CharacterIKPositionToTarget(AvatarIKGoal.RightFoot, rightFootTarget, rightFootIKWeight);
        else Debug.LogWarning("RightFootTarget Transformation is null");

        //Left Leg
        if (leftKneeTarget) CharacterIKHintPositionToTarget(AvatarIKHint.LeftKnee, leftKneeTarget, leftKneeIKWeight);
        else Debug.LogWarning("LeftKneeTarget Transformation is null");
        if (leftFootTarget) CharacterIKPositionToTarget(AvatarIKGoal.LeftFoot, leftFootTarget, leftFootIKWeight);
        else Debug.LogWarning("LeftFootTarget Transformation is null");
    }

    //controls the direction of where the head is facing
    private void CharacterLookAtTarget(Transform target, float ikWeight)
    {
        animator.SetLookAtPosition(target.position);
        animator.SetLookAtWeight(ikWeight);
    }

    //positions the hands and feet
    private void CharacterIKPositionToTarget(AvatarIKGoal goal, Transform target, float ikWeight)
    {
        animator.SetIKPosition(goal, target.position);
        animator.SetIKPositionWeight(goal, ikWeight);
    }

    //positions the knees and elbows
    private void CharacterIKHintPositionToTarget(AvatarIKHint hint, Transform target, float ikWeight)
    {
        animator.SetIKHintPosition(hint, target.position);
        animator.SetIKHintPositionWeight(hint, ikWeight);
    }

     /// <summary>
    /// Tell this character to IK–aim its RightHand at `target` with a full weight.
    /// </summary>
    public void GrabAt(Transform target)
    {
        leftHandTarget     = target;
        leftHandIKWeight   = 1f;
        // (optionally you could ramp weight up over time)
    }

    /// <summary>
    /// Release the hand back to its default animation.
    /// </summary>
    public void ReleaseHand()
    {
        leftHandIKWeight = 0f;
    }
}
