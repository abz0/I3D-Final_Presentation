// IKTriggerBehaviour.cs
using UnityEngine;
using UnityEngine.Playables;

public class IKTriggerBehaviour : PlayableBehaviour
{
    public CharacterPuppetry puppetry;

    [Header("Head")]
    public Transform headTarget;
    public float headIKWeight;

    [Header("Right Arm")]
    public Transform rightElbowTarget;
    public float rightElbowIKWeight;
    public Transform rightHandTarget;
    public float rightHandIKWeight;

    [Header("Left Arm")]
    public Transform leftElbowTarget;
    public float leftElbowIKWeight;
    public Transform leftHandTarget;
    public float leftHandIKWeight;

    [Header("Right Leg")]
    public Transform rightKneeTarget;
    public float rightKneeIKWeight;
    public Transform rightFootTarget;
    public float rightFootIKWeight;

    [Header("Left Leg")]
    public Transform leftKneeTarget;
    public float leftKneeIKWeight;
    public Transform leftFootTarget;
    public float leftFootIKWeight;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (puppetry == null) return;

        puppetry.PuppetAll(
            headTarget, headIKWeight,
            rightElbowTarget, rightElbowIKWeight,
            rightHandTarget, rightHandIKWeight,
            leftElbowTarget, leftElbowIKWeight,
            leftHandTarget, leftHandIKWeight,
            rightKneeTarget, rightKneeIKWeight,
            rightFootTarget, rightFootIKWeight,
            leftKneeTarget, leftKneeIKWeight,
            leftFootTarget, leftFootIKWeight
        );
    }

    // instead of instantly nulling everything here, just ask for a smooth release:
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (puppetry == null) return;
        puppetry.ReleaseAllSmooth();
    }
}