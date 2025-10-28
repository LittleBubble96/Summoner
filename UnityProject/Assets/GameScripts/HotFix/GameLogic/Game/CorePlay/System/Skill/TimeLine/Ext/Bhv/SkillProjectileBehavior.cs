using UnityEngine;

namespace GameLogic.Game
{
    public class SkillProjectileBehavior : SkillBehavior
    {
        public ProjectileClipData SkillProjectileData { get; set; }

        protected override void OnInit()
        {
            StartTime = SkillProjectileData.startTime;
            Duration = SkillProjectileData.duration;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CharacterElement characterElement = GetCharacter();
            if (characterElement == null)
            {
                return;
            }
            Vector3 worldPos = characterElement.GetPosition() + SkillProjectileData.position;
            Quaternion worldRot = Quaternion.Euler(SkillProjectileData.rotation) * Quaternion.Euler(0,characterElement.GetRotation().y,0);
            ProjectileManager.Instance.CreateProjectile<Projectile>(
                SkillProjectileData.projectileId,worldPos,worldRot * Vector3.forward,OwnerActorInstanceId);
        }

        public override void Clear()
        {
            base.Clear();
            SkillProjectileData = null;
        }
    }
}