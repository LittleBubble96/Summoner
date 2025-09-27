using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class ProjectileView : IProjectileView
    {
        private List<ProjectItemView> ProjectileViews = new List<ProjectItemView>();
        public void CreateProjectile(Projectile projectile)
        {
            GameObject projectileObj = PoolManager.Instance.GetGameObject(projectile.ProjectileConfig.ProjectileRes);
            ProjectItemView projectileView = projectileObj.GetComponent<ProjectItemView>();
            if (projectileView == null)
            {
                projectileView = projectileObj.AddComponent<ProjectItemView>();
            }
            projectileView.InitProjectile(projectile);
        }

        public void ClearScene()
        {
            
        }

    }
}