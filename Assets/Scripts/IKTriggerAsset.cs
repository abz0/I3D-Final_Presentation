using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class IKTriggerAsset : PlayableAsset
{
    public ExposedReference<CharacterPuppetry> puppetry;
    public ExposedReference<Transform> handTarget;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<IKTriggerBehaviour>.Create(graph);

        var behaviour = playable.GetBehaviour();
        behaviour.puppetry = puppetry.Resolve(graph.GetResolver());
        behaviour.handTarget = handTarget.Resolve(graph.GetResolver());

        return playable;
    }
    
}
