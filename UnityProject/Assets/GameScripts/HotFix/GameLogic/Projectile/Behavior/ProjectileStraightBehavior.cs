using GameConfig.projectile;
using UnityEngine;

namespace GameLogic
{
    public class ProjectileStraightBehavior : ProjectileBehavior
    {
        private float _speed;
        private float _gravity;
        private float _totalTime;
        protected override void OnInit(ProjectileConfig projectileConfig)
        {
            base.OnInit(projectileConfig);
            _speed = projectileConfig.Speed;
            _gravity = projectileConfig.Gravity;
        }

        public override void DoUpdate(float dt)
        {
            base.DoUpdate(dt);
            Vector3 normalizeDir = Direction.normalized;
            _totalTime += dt;
            Current += dt * _speed * normalizeDir;
            Current -= dt * _gravity * Vector3.down;
            //方向为抛物线切线方向
            CurrentDirection = (normalizeDir * _speed - _gravity * _totalTime * Vector3.up).normalized;
        }
    }
}