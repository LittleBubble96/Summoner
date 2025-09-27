using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

[SerializeField]
public class AnimationParameterBehaviour : PlayableBehaviour
{
    [SerializeField]
    public AnimationClip animationClip;
    [SerializeField]
    public float speed = 1f;
    public float weight = 1f;
    public bool loop;

    private Animator animator;
    private int clipHash;

    public override void OnGraphStart(Playable playable)
    {
        clipHash = Animator.StringToHash(animationClip.name);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        animator = playerData as Animator;
        if (animator == null || animationClip == null) return;

        float time = (float)playable.GetTime();
        if (loop)
        {
            time = Mathf.Repeat(time, animationClip.length);
        }
        else
        {
            time = Mathf.Clamp(time, 0, animationClip.length);
        }

        time = time * speed;
        AnimationMode.StopAnimationMode();
        AnimationMode.StartAnimationMode();
        AnimationMode.SampleAnimationClip(animator.gameObject, animationClip, time / animationClip.length);
        animator.Play(clipHash, -1, time / animationClip.length);
        animator.speed = speed;
    }
}