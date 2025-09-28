using GameConfig.projectile;
using GameFramework;
using UnityEngine;

namespace GameLogic
{
    public class ProjectileBehavior : IReference
    {
        protected Vector3 StartPosition;
        protected Vector3 Direction;
        public Vector3 Current { get; set; }
        public Vector3 CurrentDirection { get; set; }

        public void Init(Vector3 startPos , Vector3 direction, ProjectileConfig projectileConfig)
        {
            StartPosition = startPos;
            Direction = direction;
            Current = StartPosition;
            CurrentDirection = direction;
            OnInit(projectileConfig);
        }

        public virtual void Clear()
        {
            StartPosition = Vector3.zero;
            Direction = Vector3.zero;
            Current = Vector3.zero;
            CurrentDirection = Vector3.zero;
        }

        protected virtual void OnInit(ProjectileConfig projectileConfig)
        {
            
        }
        
        public virtual void DoUpdate(float dt)
        {
            
        }
    }

    public enum EBehaviorType
    {
        Straight = 1,
    }
}