using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

[SerializeField]
public class ProjectileParameterBehaviour : PlayableBehaviour
{
    [SerializeField]
    public int projectileId;
    [SerializeField]
    public Vector3 position;
    public Vector3 rotation;
    
    public override void OnGraphStart(Playable playable)
    {
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
    }
}