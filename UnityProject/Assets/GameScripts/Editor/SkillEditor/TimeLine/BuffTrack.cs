using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.76f, 0.23f, 0.96f)]
[TrackClipType(typeof(BuffClip))]
[TrackBindingType(typeof(Character))] // 假设我们有一个Character组件
public class BuffTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<BuffMixerBehaviour>.Create(graph, inputCount);
    }
}

public class BuffClip : PlayableAsset, ITimelineClipAsset
{
    public BuffBehaviour template = new BuffBehaviour();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<BuffBehaviour>.Create(graph, template);
        return playable;
    }
}

public class BuffBehaviour : PlayableBehaviour
{
    public int buffId;
    public float[] parameters;
    public bool isPermanent;

    private Character character;
    private int appliedBuffInstanceId = -1;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        character = info.output.GetReferenceObject() as Character;
        if (character != null)
        {
            // 应用Buff
            appliedBuffInstanceId = character.ApplyBuff(buffId, parameters, isPermanent);
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (character != null && appliedBuffInstanceId != -1 && !isPermanent)
        {
            // 移除Buff
            character.RemoveBuff(appliedBuffInstanceId);
            appliedBuffInstanceId = -1;
        }
    }
}

public class BuffMixerBehaviour : PlayableBehaviour
{
    // 混合逻辑如果需要
}

// 角色接口示例
public class Character : MonoBehaviour
{
    public int ApplyBuff(int buffId, float[] parameters, bool isPermanent)
    {
        // 实现应用Buff的逻辑
        return buffId; // 返回Buff实例ID
    }

    public void RemoveBuff(int buffInstanceId)
    {
        // 实现移除Buff的逻辑
    }
}
