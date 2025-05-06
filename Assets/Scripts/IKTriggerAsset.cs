using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class IKTriggerAsset : PlayableAsset
{
    public ExposedReference<CharacterPuppetry> puppetry;

    [Header("Head")]
    public ExposedReference<Transform> headTarget;
    public float headIKWeight = 0f;

    [Header("Right Arm")]
    public ExposedReference<Transform> rightElbowTarget;
    public float rightElbowIKWeight = 0f;
    public ExposedReference<Transform> rightHandTarget;
    public float rightHandIKWeight  = 0f;

    [Header("Left Arm")]
    public ExposedReference<Transform> leftElbowTarget;
    public float leftElbowIKWeight = 0f;
    public ExposedReference<Transform> leftHandTarget;
    public float leftHandIKWeight  = 0f;

    [Header("Right Leg")]
    public ExposedReference<Transform> rightKneeTarget;
    public float rightKneeIKWeight = 0f;
    public ExposedReference<Transform> rightFootTarget;
    public float rightFootIKWeight = 0f;

    [Header("Left Leg")]
    public ExposedReference<Transform> leftKneeTarget;
    public float leftKneeIKWeight = 0f;
    public ExposedReference<Transform> leftFootTarget;
    public float leftFootIKWeight = 0f;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable  = ScriptPlayable<IKTriggerBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.puppetry = puppetry.Resolve(graph.GetResolver());

        behaviour.headTarget         = headTarget.Resolve(graph.GetResolver());
        behaviour.headIKWeight       = headIKWeight;

        behaviour.rightElbowTarget   = rightElbowTarget.Resolve(graph.GetResolver());
        behaviour.rightElbowIKWeight = rightElbowIKWeight;
        behaviour.rightHandTarget    = rightHandTarget.Resolve(graph.GetResolver());
        behaviour.rightHandIKWeight  = rightHandIKWeight;

        behaviour.leftElbowTarget    = leftElbowTarget.Resolve(graph.GetResolver());
        behaviour.leftElbowIKWeight  = leftElbowIKWeight;
        behaviour.leftHandTarget     = leftHandTarget.Resolve(graph.GetResolver());
        behaviour.leftHandIKWeight   = leftHandIKWeight;

        behaviour.rightKneeTarget    = rightKneeTarget.Resolve(graph.GetResolver());
        behaviour.rightKneeIKWeight  = rightKneeIKWeight;
        behaviour.rightFootTarget    = rightFootTarget.Resolve(graph.GetResolver());
        behaviour.rightFootIKWeight  = rightFootIKWeight;

        behaviour.leftKneeTarget     = leftKneeTarget.Resolve(graph.GetResolver());
        behaviour.leftKneeIKWeight   = leftKneeIKWeight;
        behaviour.leftFootTarget     = leftFootTarget.Resolve(graph.GetResolver());
        behaviour.leftFootIKWeight   = leftFootIKWeight;

        return playable;
    }
}
