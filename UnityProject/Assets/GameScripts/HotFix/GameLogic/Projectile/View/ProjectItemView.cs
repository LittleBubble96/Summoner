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
                for (int i = 0; i < raycastResults.Length; i++)
                {
                    CharacterBaseView characterBaseView = raycastResults[i].transform.GetComponentInParent<CharacterBaseView>();
                    // if (characterBaseView == null || CharacterManager.Instance.GetRelation(_projectile.ProjectileData.))
                    // {
                    //     
                    // }
                    float dis = Vector3.Distance(startPos, raycastResults[i].point);
                    if (dis < minDis)
                    {
                        minDis = dis;
                        hitIndex = i;
                    }
                }

                if (hitIndex != -1)
                {
                    RaycastHit hitInfo = raycastResults[hitIndex];
                    if (hitInfo.transform.GetComponentInParent<CharacterBaseView>())
                    {
                        
                    }
                }
                // //命中
                // _projectile.OnHit(hitInfo.point, hitInfo.normal);
                // //播放命中特效
                // EffectManager.Instance.PlayFixedEffect(_projectile.ProjectileConfig.HitEffect,
                //     hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            }
            transform.position = _projectile.CurrentPosition;
            transform.forward = _projectile.CurrentDirection;
        }
    }
}