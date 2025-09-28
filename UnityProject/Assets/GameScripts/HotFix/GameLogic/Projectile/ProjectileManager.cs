using System.Collections.Generic;
using GameBase;
using GameConfig.projectile;
using GameFramework;
using GameProto;
using UnityEngine;

namespace GameLogic.Game
{
    public class ProjectileManager : BaseLogicSys<ProjectileManager>
    {
        private IProjectileView _projectileView;
        private Dictionary<ProjectileInstanceId,Projectile> _projectiles;
        private Queue<ProjectileInstanceId> _destroyQueue;

        public override bool OnInit()
        {
            _projectiles = new Dictionary<ProjectileInstanceId, Projectile>();
            _destroyQueue = new Queue<ProjectileInstanceId>();
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
            ProjectileInstanceId instanceId = ProjectileInstanceId.NewId();
            ProjectileConfig config = ConfigSystem.Instance.Tables.TbProjectile.Get(projectId);
            projectile.Init(instanceId,config, position, direction);
            _projectiles.Add(instanceId,projectile);
            _projectileView.CreateProjectile(projectile);
        }

        public void DestroyProjectile(ProjectileInstanceId instanceId)
        {
            if (_projectiles.TryGetValue(instanceId,out var projectile))
            {
                projectile.DestroySelf();
                _projectileView.DestroyProjectile(instanceId);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_projectiles != null)
            {
                foreach (var projectile in _projectiles)
                {
                    projectile.Value.DoUpdate(Time.deltaTime);
                    if (projectile.Value.State == ProjectileState.Destroyed)
                    {
                        _destroyQueue.Enqueue(projectile.Key);
                    }
                }
            }
            _projectileView?.DoUpdate(Time.deltaTime);
            ProcessDestroyQueue();
        }

        private void ProcessDestroyQueue()
        {
            while (_destroyQueue.Count > 0)
            {
                ProjectileInstanceId id = _destroyQueue.Dequeue();
                if (_projectiles.TryGetValue(id, out var projectile))
                {
                    _projectiles.Remove(id);
                    ReferencePool.Release(projectile);
                    _projectileView.RealDestroyProjectile(id);
                }
            }
        }

        public void ClearScene()
        {
            foreach (var projectile in _projectiles)
            {
                ReferencePool.Release(projectile.Value);
            }
            _projectiles.Clear();
            _projectileView.ClearScene();
            ProjectileInstanceIdGenerator.ClearCache();
        }
    }
}