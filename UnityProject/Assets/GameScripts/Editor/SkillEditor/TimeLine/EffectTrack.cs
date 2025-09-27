using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.22f, 0.65f, 0.96f)]
[TrackClipType(typeof(EffectClip))]
[TrackBindingType(typeof(Transform))]
public class EffectTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<EffectMixerBehaviour>.Create(graph, inputCount);
    }
}

public class EffectClip : PlayableAsset, ITimelineClipAsset
{
    public EffectBehaviour template = new EffectBehaviour();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<EffectBehaviour>.Create(graph, template);
        return playable;
    }
}

public class EffectBehaviour : PlayableBehaviour
{
    public GameObject effectPrefab;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;
    public float scale = 1f;
    public bool followTarget;
    public string attachPoint;

    private GameObject spawnedEffect;
    private Transform parentTransform;
    private Transform attachPointTransform;

    public override void OnPlayableCreate(Playable playable)
    {
        // 初始化逻辑
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (effectPrefab != null && spawnedEffect == null)
        {
            parentTransform = info.output.GetReferenceObject() as Transform;
            if (parentTransform != null)
            {
                // 查找附着点
                if (!string.IsNullOrEmpty(attachPoint) && parentTransform.Find(attachPoint) != null)
                {
                    attachPointTransform = parentTransform.Find(attachPoint);
                }
                else
                {
                    attachPointTransform = parentTransform;
                }

                // 实例化特效
                spawnedEffect = GameObject.Instantiate(effectPrefab, 
                    attachPointTransform.position + positionOffset, 
                    attachPointTransform.rotation * rotationOffset,
                    followTarget ? attachPointTransform : null);
                spawnedEffect.transform.localScale = Vector3.one * scale;
            }
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (spawnedEffect != null)
        {
            GameObject.Destroy(spawnedEffect);
            spawnedEffect = null;
        }
    }
}

public class EffectMixerBehaviour : PlayableBehaviour
{
    // 混合逻辑如果需要
}
