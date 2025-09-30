using UnityEngine;

namespace GameLogic.Game
{
    public interface IProjectileView
    {
        void CreateProjectile(Projectile projectile);

        void DoUpdate(float dt);
        
        //销毁子弹 但不是立即销毁
        void DestroyProjectile(ProjectileInstanceId instanceId);
        //实际销毁子弹 立即销毁
        void RealDestroyProjectile(ProjectileInstanceId instanceId);
        //播放击中特效
        void PlayProjectileHit(ProjectileInstanceId instanceId,RaycastHit hit);

        void ClearScene();
    }
}