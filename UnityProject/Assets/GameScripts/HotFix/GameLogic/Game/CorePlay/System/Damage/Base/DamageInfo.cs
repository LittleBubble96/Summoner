using System.Collections.Generic;
using GameFramework;

namespace GameLogic.Game
{
    //伤害
    public class DamageElement : IReference
    {
        public DamageInfo DamageInfo { get; set; }
        private Dictionary<ActorInstanceId, DamageData> _damageTargets;
        private Queue<ActorInstanceId> _destroyQueue;

        public void InitDamage(ActorInstanceId owner,DamageSourceType sourceType ,DamageType damageType ,float internalTime)
        {
            DamageInfo = new DamageInfo()
            {
                DamageSourceActorId = owner,
                DamageSourceType = sourceType,
                DamageType = damageType,
                DamageInternal = internalTime,
            };
            _damageTargets = new Dictionary<ActorInstanceId, DamageData>();
            _destroyQueue = new Queue<ActorInstanceId>();
        }

        public void DoUpdate(float dt)
        {
            foreach (var damageData in _damageTargets)
            {
                damageData.Value.TimeCount -= dt;
                if (damageData.Value.TimeCount <= 0)
                {
                    _destroyQueue.Enqueue(damageData.Key);
                }
            }
            ProcessDestroyQueue();
        }

        private void ProcessDestroyQueue()
        {
            while (_destroyQueue.Count > 0)
            {
                ActorInstanceId actorInstanceId = _destroyQueue.Dequeue();
                if (_damageTargets.TryGetValue(actorInstanceId, out var damageData))
                {
                    ReferencePool.Release(damageData);
                    _damageTargets.Remove(actorInstanceId);
                }
            }
        }

        public void DamageTarget(ActorInstanceId target,DamageSourceType sourceType ,DamageType damageType)
        {
            if (_damageTargets.ContainsKey(target))
            {
                //还在作用时间内 无伤害
                return;
            }
            //立即执行
            CharacterManager.Instance.DamageCharacter(target,DamageHelper.CalDamageValue(target,sourceType,damageType),sourceType);
            DamageData damageData = ReferencePool.Acquire<DamageData>();
            damageData.Init(DamageInfo.DamageInternal);
            _damageTargets.Add(target,damageData);
        }

        public void Clear()
        {
            DamageInfo = default;
            _damageTargets.Clear();
        }
    }

    public class DamageData : IReference
    {
        public float TimeCount;

        public void Init(float timer)
        {
            TimeCount = timer;
        }

        public void Clear()
        {
            TimeCount = 0;
        }
    }

    public struct DamageInfo
    {
        public uint DamageId;
        //伤害间隔
        public float DamageInternal;
        //伤害来源角色id
        public ActorInstanceId DamageSourceActorId;
        //伤害来源
        public DamageSourceType DamageSourceType;
        //伤害类型
        public DamageType DamageType;
    }

    public enum DamageSourceType
    {
        None = 0,
        PhysicalAttack = 1 << 0, //物理攻击
        MagicAttack = 1 << 1, //魔法攻击
        TrueAttack = 1 << 2, //真实伤害
    }

    public enum DamageType
    {
        Damage,//伤害
        Treat,//治疗
    }

}