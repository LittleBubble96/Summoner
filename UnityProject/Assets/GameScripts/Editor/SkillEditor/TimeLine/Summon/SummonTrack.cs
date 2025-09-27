using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.23f, 0.96f, 0.58f)]
[TrackClipType(typeof(SummonClip))]
[TrackBindingType(typeof(Transform))]
public class SummonTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<SummonMixerBehaviour>.Create(graph, inputCount);
    }
}

public class SummonBehaviour : PlayableBehaviour
{
    public int summonId;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;

    private GameObject summonedObject;
    private Transform casterTransform;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        casterTransform = info.output.GetReferenceObject() as Transform;
        // if (casterTransform != null && summonPrefab != null && summonedObject == null)
        // {
        //     // 计算生成位置
        //     Vector3 spawnPosition = casterTransform.position + positionOffset;
        //     Quaternion spawnRotation = casterTransform.rotation * rotationOffset;
        //
        //     // 实例化召唤物
        //     summonedObject = GameObject.Instantiate(summonPrefab, spawnPosition, spawnRotation);
        //     
        //     // 设置行为
        //     SummonBehaviourController controller = summonedObject.GetComponent<SummonBehaviourController>();
        //     if (controller != null)
        //     {
        //         controller.Initialize(behaviorType, behaviorParameters);
        //     }
        //
        //     // 设置生命周期
        //     if (lifetime > 0)
        //     {
        //         Object.Destroy(summonedObject, lifetime);
        //     }
        // }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        // 如果需要在片段结束时销毁召唤物，可以在这里处理
        // if (summonedObject != null && lifetime <= 0)
        // {
        //     Destroy(summonedObject);
        //     summonedObject = null;
        // }
    }
}

public class SummonMixerBehaviour : PlayableBehaviour
{
    // 混合逻辑如果需要
}

public class SummonBehaviourController : MonoBehaviour
{
    public void Initialize(string behaviorType, float[] parameters)
    {
        // 实现召唤物行为逻辑
    }
}
