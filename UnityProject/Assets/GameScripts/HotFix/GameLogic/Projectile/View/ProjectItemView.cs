using GameLogic.Game.Effect;
using UnityEngine;

namespace GameLogic
{
    public class ProjectItemView : MonoBehaviour
    {
        private Projectile _projectile;
        public void InitProjectile(Projectile projectile)
        {
            _projectile = projectile;
            transform.position = _projectile.CurrentPosition;
            transform.forward = _projectile.CurrentDirection;
            //播放枪口特效
            EffectManager.Instance.PlayFixedEffect(_projectile.ProjectileConfig.ShootEffect,
                _projectile.CurrentPosition, Quaternion.LookRotation(_projectile.CurrentDirection));
        }

        public void DoUpdate(float dt)
        {
            transform.position = _projectile.CurrentPosition;
            transform.forward = _projectile.CurrentDirection;
        }
    }
}