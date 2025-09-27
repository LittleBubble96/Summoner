using System.Collections.Generic;
using GameBase;
using GameConfig.projectile;
using GameFramework;
using GameProto;
using UnityEngine;

namespace GameLogic
{
    public class ProjectileManager : BaseLogicSys<ProjectileManager>
    {
        private IProjectileView _projectileView;
        private List<Projectile> _projectiles;

        public override bool OnInit()
        {
            _projectiles = new List<Projectile>();
            return base.OnInit();
        }

        public void Inject(IProjectileView projectileView)
        {
            _projectileView = projectileView;
        }

        /// <summary>
        /// 创建子弹
        /// </summary>
        /// <param name="projectId">子弹id</param>
        /// <param name="position">位置</param>
        /// <param name="direction">方向</param>
        public void CreateProjectile<T>(int projectId , Vector3 position , Vector3 direction) where T : Projectile, new()
        {
            Projectile projectile = ReferencePool.Acquire<T>();
            ProjectileConfig config = ConfigSystem.Instance.Tables.TbProjectile.Get(projectId);
            projectile.Init(config, position, direction);
            _projectiles.Add(projectile);
            _projectileView.CreateProjectile(projectile);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_projectiles != null)
            {
                for (int i = _projectiles.Count - 1; i >= 0; i--)
                {
                    _projectiles[i].DoUpdate(Time.deltaTime);
                }
            }
        }
    }
}