using System.Collections.Generic;
using GameBase;
using GameFramework;

namespace GameLogic.Game
{
    public class DamageManger : BaseLogicSys<DamageManger>
    {
        public Dictionary<uint, DamageElement> DamageElements = new Dictionary<uint, DamageElement>();

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public uint CreateDamageIns(ActorInstanceId owner, DamageSourceType damageSourceType , DamageType damageType , float internalTime)
        {
            DamageElement damageElement = ReferencePool.Acquire<DamageElement>();
            damageElement.InitDamage(owner , damageSourceType ,damageType ,internalTime);
            return 1;
        }

        public void Damage()
        {
            
        }
    }
}