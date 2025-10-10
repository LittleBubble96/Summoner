using GameFramework;

namespace GameLogic.Game
{
    public class AnimationTrack : SkillTrack
    {
        public override void AddBehavior(SkillBehaviorData behaviorData)
        {
            if (behaviorData is AnimationClipData animationData)
            {
                SkillAnimationBehavior animationBehavior = ReferencePool.Acquire<SkillAnimationBehavior>();
                animationBehavior.AnimationData = animationData;
                AddBehaviorInList(animationBehavior);
            }
        }
        
    }
}