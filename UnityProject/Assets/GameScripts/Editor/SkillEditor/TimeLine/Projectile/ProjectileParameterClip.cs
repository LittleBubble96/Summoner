using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class ProjectileParameterClip : PlayableAsset, ITimelineClipAsset
{
    public AnimationClip animationClip;
    public float speed = 1f;
    public ProjectileParameterBehaviour template = new ProjectileParameterBehaviour();

    private float m_speed = -1f;
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ProjectileParameterBehaviour>.Create(graph, template);
        var behaviour = playable.GetBehaviour();
        return playable;
    }
    
    // 重写此方法，动态计算片段长度
    public override double duration
    {
        get
        {
            // 确保动画片段和速度有效
            if (animationClip == null || speed <= 0)
                return base.duration; // 默认为1秒（无效状态）
            
            // 核心逻辑：片段长度 = 动画原始时长 / 速度
            return animationClip.length / speed;
        }
    }

    private void OnValidate()
    {
        if (m_speed != speed)
        {
            m_speed = speed;
            if (template != null)
            {
                // template.speed = speed;
                //修改轨道时长
                if (animationClip != null && speed != 0)
                {
#if UNITY_EDITOR
                    UnityEditor.Timeline.TimelineEditor.Refresh(UnityEditor.Timeline.RefreshReason.ContentsModified);
#endif
                }
            }
        }
    }
}