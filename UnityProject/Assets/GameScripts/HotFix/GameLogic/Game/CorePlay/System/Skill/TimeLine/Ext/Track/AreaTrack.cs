using GameFramework;

namespace GameLogic.Game
{
    public class AreaTrack : SkillTrack
    {
        public override void AddBehavior(SkillBehaviorData behaviorData)
        {
            if (behaviorData is AreaData areaData)
            {
                SkillAreaBehavior projectileBehavior = ReferencePool.Acquire<SkillAreaBehavior>();
                projectileBehavior.Area = areaData;
                AddBehaviorInList(projectileBehavior);
            }
        }
    }
}