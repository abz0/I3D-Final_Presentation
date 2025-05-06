using UnityEngine;
using UnityEngine.Playables;

public class IKTriggerBehaviour : PlayableBehaviour
{
    public CharacterPuppetry puppetry;
    public Transform handTarget;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (puppetry != null && handTarget != null)
        {
            puppetry.GrabAt(handTarget);
        }
    }
}
