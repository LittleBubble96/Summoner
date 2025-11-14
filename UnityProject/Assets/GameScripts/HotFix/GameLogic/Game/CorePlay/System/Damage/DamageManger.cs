using System.Collections.Generic;
using GameBase;
using GameFramework;
using UnityEngine;

namespace GameLogic.Game
{
    public class DamageManger : BaseLogicSys<DamageManger>
    {
        public Dictionary<uint, DamageElement> DamageElements = new Dictionary<uint, DamageElement>();
        private Queue<uint> _destroyQueue = new Queue<uint>();

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var damage in DamageElements)
            {
                damage.Value.DoUpdate(Time.deltaTime);
            }

            ProcessDestroyQueue();
        }

        private void ProcessDestroyQueue()
        {
            while (_destroyQueue.Count > 0)
            {
                uint destroyId = _destroyQueue.Dequeue();
                if (DamageElements.TryGetValue(destroyId , out var damageElement))
                {
                    ReferencePool.Release(damageElement);
                    DamageElements.Remove(destroyId);
                }
            }
        }

        //创建伤害实例
        public uint CreateDamageIns(ActorInstanceId owner, DamageSourceType damageSourceType , DamageType damageType , float internalTime)
        {
            uint damageId = DamageIdGenerator.NewId();
            DamageElement damageElement = ReferencePool.Acquire<DamageElement>();
            damageElement.InitDamage(damageId,owner , damageSourceType ,damageType ,internalTime);
            return damageId;
        }

        //销毁伤害实例
        public void DestroyDamageIns(uint destroyId)
        {
            _destroyQueue.Enqueue(destroyId);
        }

        //是否可以收到伤害
        public bool CanDamage(uint destroyId , ActorInstanceId target)
        {
            if (DamageElements.TryGetValue(destroyId, out var damageElement))
            {
                return damageElement.CanDamageTarget(target);
            }
            return false;
        }

        //伤害
        public void Damage(uint destroyId , ActorInstanceId target)
        {
            if (DamageElements.TryGetValue(destroyId, out var damageElement))
            {
                damageElement.DamageTarget(target);
            }
        }
    }
}