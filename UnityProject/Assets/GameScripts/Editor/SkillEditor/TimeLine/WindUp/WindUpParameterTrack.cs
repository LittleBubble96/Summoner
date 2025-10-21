using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(WindUpParameterClip))]
public class WindUpParameterTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<WindUpParameterMixerBehaviour>.Create(graph, inputCount);
    }
}



public class WindUpParameterMixerBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        // int inputCount = playable.GetInputCount();
        // Animator animator = playerData as Animator;
        //
        // if (animator == null) return;
        //
        // for (int i = 0; i < inputCount; i++)
        // {
        //     float inputWeight = playable.GetInputWeight(i);
        //     ScriptPlayable<AnimationParameterBehaviour> inputPlayable = 
        //         (ScriptPlayable<AnimationParameterBehaviour>)playable.GetInput(i);
        //     AnimationParameterBehaviour input = inputPlayable.GetBehaviour();
        //
        //     if (input.animationClip != null && inputWeight > 0)
        //     {
        //         float time = (float)inputPlayable.GetTime();
        //         if (input.loop)
        //         {
        //             time = Mathf.Repeat(time, input.animationClip.length);
        //         }
        //         else
        //         {
        //             time = Mathf.Clamp(time, 0, input.animationClip.length);
        //         }
        //         //速度缩放时间
        //         time = time / input.speed;
        //         AnimationMode.StopAnimationMode();
        //         AnimationMode.StartAnimationMode();
        //         AnimationMode.SampleAnimationClip(animator.gameObject, input.animationClip, time / input.animationClip.length);
        //         
        //         // animator.Play(clipHash, -1, time / input.animationClip.length);
        //         // animator.speed = input.speed;
        //     }
        // }
    }
}
