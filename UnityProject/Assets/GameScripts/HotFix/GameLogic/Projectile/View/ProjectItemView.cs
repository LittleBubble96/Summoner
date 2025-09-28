using GameLogic.Game.Effect;
using UnityEngine;

namespace GameLogic.Game
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
            Vector3 startPos = transform.position;
            Vector3 endPos = _projectile.CurrentPosition;
            //射线检测
            Ray ray = new Ray(startPos, endPos - startPos);
            
            transform.position = _projectile.CurrentPosition;
            transform.forward = _projectile.CurrentDirection;
        }
    }
}