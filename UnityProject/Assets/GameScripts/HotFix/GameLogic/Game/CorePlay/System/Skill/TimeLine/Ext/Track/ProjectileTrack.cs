using GameFramework;

namespace GameLogic.Game
{
    public class ProjectileTrack : SkillTrack
    {
        public override void AddBehavior(SkillBehaviorData behaviorData)
        {
            if (behaviorData is ProjectileClipData projectileData)
            {
                SkillProjectileBehavior projectileBehavior = ReferencePool.Acquire<SkillProjectileBehavior>();
                projectileBehavior.SkillProjectileData = projectileData;
                AddBehaviorInList(projectileBehavior);
            }
        }
    }

}