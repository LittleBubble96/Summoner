using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class WindUpParameterClip : PlayableAsset, ITimelineClipAsset
{
    public WindUpParameterBehaviour template = new WindUpParameterBehaviour();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<WindUpParameterBehaviour>.Create(graph, template);
        return playable;
    }
    
}