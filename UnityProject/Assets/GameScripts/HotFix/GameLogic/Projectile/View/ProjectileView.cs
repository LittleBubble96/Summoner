using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.Game
{
    public class ProjectileView : IProjectileView
    {
        private Dictionary<ProjectileInstanceId, ProjectItemView> _projectileViews =
            new Dictionary<ProjectileInstanceId, ProjectItemView>();

        #region 接口

        public void CreateProjectile(Projectile projectile)
        {
            GameObject projectileObj = PoolManager.Instance.GetGameObject(projectile.ProjectileConfig.ProjectileRes);
            ProjectItemView projectileView = projectileObj.GetComponent<ProjectItemView>();
            if (projectileView == null)
            {
                projectileView = projectileObj.AddComponent<ProjectItemView>();
            }
            projectileView.InitProjectile(projectile);
            _projectileViews.Add(projectile.InstanceId, projectileView);
        }

        public void DoUpdate(float dt)
        {
            foreach (var projectileView in _projectileViews.Values)
            {
                projectileView.DoUpdate(dt);
            }
        }
        
        //销毁前调用
        public void DestroyProjectile(ProjectileInstanceId instanceId)
        {
            //这里可以做一些销毁前的特效播放等操作
        }

        public void RealDestroyProjectile(ProjectileInstanceId instanceId)
        {
            //实际销毁
            if (_projectileViews.TryGetValue(instanceId,out var projectileView))
            {
                PoolManager.Instance.PushObject(projectileView);
                _projectileViews.Remove(instanceId);
            }
        }

        public void PlayProjectileHit(ProjectileInstanceId instanceId, RaycastHit hit)
        {
            if (_projectileViews.TryGetValue(instanceId,out var projectItemView))
            {
                projectItemView.PlayHitEffect(hit);
            }
        }

        public void ClearScene()
        {
            foreach (var projectItemView in _projectileViews)
            {
                PoolManager.Instance.PushObject(projectItemView.Value);
            }
            _projectileViews.Clear();
        }

        #endregion
        

    }
}