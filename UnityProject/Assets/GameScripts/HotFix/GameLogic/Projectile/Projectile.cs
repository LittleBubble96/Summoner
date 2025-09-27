using GameConfig.projectile;
using GameFramework;
using UnityEngine;

namespace GameLogic
{
    public class Projectile : IReference
    {
        protected ProjectileBehavior Behavior;
        //当前位置
        public Vector3 CurrentPosition => Behavior.Current;
        public Vector3 CurrentDirection => Behavior.CurrentDirection;
        public ProjectileConfig ProjectileConfig { get; set; }

        public void Init(ProjectileConfig projectileConfig, Vector3 position , Vector3 direction)
        {
            CreateBehavior(projectileConfig.MoveType,position,direction,projectileConfig);
            ProjectileConfig = projectileConfig;
        }

        public void DoUpdate(float dt)
        {
            if (Behavior != null)
            {
                Behavior.DoUpdate(dt);
            }
        }

        private void CreateBehavior(int behaviorType, Vector3 position , Vector3 direction,ProjectileConfig projectileConfig)
        {
            EBehaviorType type = (EBehaviorType)behaviorType;
            switch (type)
            {
                case EBehaviorType.Straight:
                    Behavior = new ProjectileStraightBehavior();
                    break;
                default:
                    Behavior = new ProjectileBehavior();
                    break;
            }
            Behavior.Init(position,direction,projectileConfig);
        }

        public void Clear()
        {
            ReferencePool.Release(Behavior);
        }
    }
}