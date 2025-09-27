using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SummonClip : PlayableAsset, ITimelineClipAsset
{
    public int summonId;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;
    public SummonBehaviour template = new SummonBehaviour();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SummonBehaviour>.Create(graph, template);
        return playable;
    }

    private void OnValidate()
    {
        template.summonId = summonId;
        template.positionOffset = positionOffset;
        template.rotationOffset = rotationOffset;
    }
}