using UnityEngine;

namespace GameLogic
{
    public interface IProjectileView
    {
        public void CreateProjectile(Projectile projectile);

        public void ClearScene();
    }
}