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
        public void CreateProjectile<T>(int projectId , Vector3 position , Vector3 direction , ActorInstanceId ownerId) where T : Projectile, new()
        {
            Projectile projectile = ReferencePool.Acquire<T>();
            ProjectileData projectileData = ReferencePool.Acquire<ProjectileData>();
            projectileData.OwnerId = ownerId;
            ProjectileInstanceId instanceId = ProjectileInstanceId.NewId();
            ProjectileConfig config = ConfigSystem.Instance.Tables.TbProjectile.Get(projectId);
            projectile.Init(instanceId,config, position, direction ,projectileData);
            _projectiles.Add(instanceId,projectile);
            _projectileView.CreateProjectile(projectile);
        }

        /// <summary>
        /// 销毁子弹
        /// </summary>
        public void DestroyProjectile(ProjectileInstanceId instanceId)
        {
            if (_projectiles.TryGetValue(instanceId,out var projectile))
            {
                projectile.DestroySelf();
                _projectileView.DestroyProjectile(instanceId);
            }
        }

        public Projectile GetProjectile(ProjectileInstanceId projectileInstanceId)
        {
            return _projectiles.TryGetValue(projectileInstanceId, out var projectile) ? projectile : null;
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
        
        //执行销毁队列
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

        /// <summary>
        /// 该角色是否可以被击中
        /// </summary>
        public bool CanHitCharacter(ProjectileInstanceId projectileInstanceId, CharacterElement characterElement)
        {
            if (characterElement == null || characterElement.IsDead())
            {
                return false;
            }
            Projectile projectile = GetProjectile(projectileInstanceId);
            if (projectile == null)
            {
                return false;
            }
            CharacterElement owner = CharacterManager.Instance.GetCharacter(projectile.ProjectileData.OwnerId);
            if (owner == null)
            {
                return false;
            }
            FactionRelationType relation = CharacterManager.Instance.GetRelation(characterElement, owner);
            return relation == FactionRelationType.Hostile;
        }

        public void HitProjectile(ProjectileInstanceId projectileInstanceId ,RaycastHit hitInfo , CharacterElement characterElement)
        {
            
        }

        //清除场景
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