using GameFramework;

namespace GameLogic.Game
{
    public class SkillWindDownTrack : SkillTrack
    {
        public override void AddBehavior(SkillBehaviorData behaviorData)
        {
            if (behaviorData is SkillWindDownData data)
            {
                SkillWindDownBehavior behavior = ReferencePool.Acquire<SkillWindDownBehavior>();
                behavior.Data = data;
                AddBehaviorInList(behavior);
            }
        }
    }
}