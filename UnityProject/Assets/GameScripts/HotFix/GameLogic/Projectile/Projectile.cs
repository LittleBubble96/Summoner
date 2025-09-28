using GameConfig.projectile;
using GameFramework;
using UnityEngine;

namespace GameLogic.Game
{
    public enum ProjectileState
    {
        Running,
        Destroyed,
    }

    public class Projectile : IReference
    {
        //子弹行为
        protected ProjectileBehavior Behavior;

        //当前位置
        public Vector3 CurrentPosition => Behavior.Current;
        //子弹当前朝向
        public Vector3 CurrentDirection => Behavior.CurrentDirection;
        //子弹状态
        public ProjectileState State { get; set; } = ProjectileState.Running;

        //子弹配置
        public ProjectileConfig ProjectileConfig { get; set; }
        
        //子弹位移id
        public ProjectileInstanceId InstanceId { get; set; }

        public void Init(ProjectileInstanceId instanceId ,ProjectileConfig projectileConfig, Vector3 position , Vector3 direction)
        {
            InstanceId = instanceId;
            CreateBehavior(projectileConfig.MoveType,position,direction,projectileConfig);
            ProjectileConfig = projectileConfig;
            State = ProjectileState.Running;
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

        public void DestroySelf()
        {
            State = ProjectileState.Destroyed;
        }

        public void Clear()
        {
            ProjectileConfig = null;
            InstanceId = default;
            State = ProjectileState.Running;
            ReferencePool.Release(Behavior);
        }
    }
}