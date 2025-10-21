using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class WindDownParameterClip : PlayableAsset, ITimelineClipAsset
{
    public WindDownParameterBehaviour template = new WindDownParameterBehaviour();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<WindDownParameterBehaviour>.Create(graph, template);
        return playable;
    }
    
}