using System;
using GameLogic.Game;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class AreaParameterClip : PlayableAsset, ITimelineClipAsset
{
    public AreaType AreaType;

    public Vector3 Origin;
    // 球形/扇形共用参数
    public float Radius;
    // 盒形参数
    public Vector3 BoxSize;
    // 扇形专属参数
    public float SectorAngle; // 角度（0-360）
    public Vector3 Direction; // 朝向
    public AreaParameterBehaviour template = new AreaParameterBehaviour();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AreaParameterBehaviour>.Create(graph, template);
        playable.GetBehaviour().AreaType = AreaType;
        playable.GetBehaviour().Origin = Origin;
        playable.GetBehaviour().Radius = Radius;
        playable.GetBehaviour().BoxSize = BoxSize;
        playable.GetBehaviour().SectorAngle = SectorAngle;
        playable.GetBehaviour().Direction = Direction;

        return playable;
    }
}