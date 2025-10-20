using GameFramework;

namespace GameLogic.Game
{
    public class SkillWindUpTrack : SkillTrack
    {
        public override void AddBehavior(SkillBehaviorData behaviorData)
        {
            if (behaviorData is SkillWindUpData data)
            {
                SkillWindUpBehavior behavior = ReferencePool.Acquire<SkillWindUpBehavior>();
                behavior.Data = data;
                AddBehaviorInList(behavior);
            }
        }
    }
}