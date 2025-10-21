using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class ProjectileParameterClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    public int projectileId;
    [SerializeField]
    public Vector3 position;
    public Vector3 rotation;
    public ProjectileParameterBehaviour template = new ProjectileParameterBehaviour();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ProjectileParameterBehaviour>.Create(graph, template);
        return playable;
    }
}