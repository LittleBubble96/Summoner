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
            var raycastResults = Physics.RaycastAll(ray, Vector3.Distance(startPos, endPos), LayerDefine.LayerProjectileHit);
            if (raycastResults is { Length: > 0 })
            {
                float minDis = float.MaxValue;
                int hitIndex = -1;
                CharacterBaseView characterBaseView = null;
                for (int i = 0; i < raycastResults.Length; i++)
                {
                    CharacterBaseView characterView = raycastResults[i].transform.GetComponentInParent<CharacterBaseView>();
                    if (characterView)
                    {
                        bool canHit = ProjectileManager.Instance.CanHitCharacter(_projectile.InstanceId, characterView.CharacterElement);
                        if (!canHit)
                        {
                            continue;
                        }
                    }
                    float dis = Vector3.Distance(startPos, raycastResults[i].point);
                    if (dis < minDis)
                    {
                        minDis = dis;
                        hitIndex = i;
                        characterBaseView = characterView;
                    }
                }

                if (hitIndex != -1)
                {
                    ProjectileManager.Instance.HitProjectile(_projectile.InstanceId,raycastResults[hitIndex],characterBaseView ? characterBaseView.CharacterElement:null);
                }
            }
            transform.position = _projectile.CurrentPosition;
            transform.forward = _projectile.CurrentDirection;
        }

        public void PlayHitEffect(RaycastHit hitInfo)
        {
            EffectManager.Instance.PlayFixedEffect(_projectile.ProjectileConfig.HitEffect,
                hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        }
    }
}