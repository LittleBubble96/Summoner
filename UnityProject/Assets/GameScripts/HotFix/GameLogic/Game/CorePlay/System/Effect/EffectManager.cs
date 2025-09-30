using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.Game.Effect
{
    /// <summary>
    /// 特效管理器
    /// 管理所有特效实例，通过实例ID进行操作
    /// </summary>
    public class EffectManager : MonoBehaviour
    {
        // 单例实例
        public static EffectManager Instance { get; private set; }
        

        // 所有活跃的特效（通过实例ID索引）
        private Dictionary<EffectInstanceId, EffectBase>
            _activeEffects = new Dictionary<EffectInstanceId, EffectBase>();
        private Queue<EffectInstanceId> _recycleQueue = new Queue<EffectInstanceId>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            // 遍历所有活跃特效并更新
            float deltaTime = Time.deltaTime;
            foreach (var effect in _activeEffects.Values)
            {
                effect.OnUpdate(deltaTime);
            }
            
        }

        private void ProcessRecycleEffect()
        {
            while (_recycleQueue.Count > 0)
            {
                EffectBase effectBase = GetEffectById(_recycleQueue.Dequeue());
                if (effectBase != null)
                {
                    effectBase.Recycle();
                    OnEffectRecycle(effectBase);
                }
                
            }
        }

        /// <summary>
        /// 播放固定位置特效
        /// </summary>
        public EffectInstanceId PlayFixedEffect(string effectPrefab, Vector3 position,
            Quaternion rotation, Transform parent = null,
            float autoRecycleTime = 0)
        {
            if (effectPrefab == null)
            {
                Debug.LogError("Effect prefab is null!");
                return new EffectInstanceId(0);
            }

            // 从对象池获取或创建新特效
            FixedEffect effect = GetOrCreateEffect<FixedEffect>(effectPrefab);

            // 初始化特效
            effect.Initialize(position, rotation, parent, autoRecycleTime, RecycleEffect);

            // 添加到活跃特效字典
            _activeEffects[effect.InstanceId] = effect;

            return effect.InstanceId;
        }

        /// <summary>
        /// 播放跟随特效
        /// </summary>
        public EffectInstanceId PlayFollowEffect(string effectPrefab, Transform target,
            Vector3 offset, Quaternion rotation,
            float autoRecycleTime = 0)
        {
            if (effectPrefab == null)
            {
                Debug.LogError("Effect prefab is null!");
                return new EffectInstanceId(0);
            }

            if (target == null)
            {
                Debug.LogError("Target transform is null!");
                return new EffectInstanceId(0);
            }

            // 从对象池获取或创建新特效
            FollowEffect effect = GetOrCreateEffect<FollowEffect>(effectPrefab);

            // 初始化特效
            effect.Initialize(target, offset, rotation, autoRecycleTime, RecycleEffect);

            // 添加到活跃特效字典
            _activeEffects[effect.InstanceId] = effect;

            return effect.InstanceId;
        }

        /// <summary>
        /// 根据实例ID回收特效
        /// </summary>
        public void RecycleEffect(EffectInstanceId instanceId)
        {
            _recycleQueue.Enqueue(instanceId);
        }

        /// <summary>
        /// 根据实例ID获取特效
        /// </summary>
        public EffectBase GetEffectById(EffectInstanceId instanceId)
        {
            _activeEffects.TryGetValue(instanceId, out var effect);
            return effect;
        }

        /// <summary>
        /// 从对象池获取或创建特效
        /// </summary>
        private T GetOrCreateEffect<T>(string prefab) where T : EffectBase
        {
            // 池中没有可用特效，创建新的
            GameObject effectObj = PoolManager.Instance.GetGameObject(prefab);
            effectObj.transform.SetParent(transform); // 所有特效作为管理器的子对象
            effectObj.SetActive(false);

            T newEffect = effectObj.GetComponent<T>();
            if (newEffect == null)
            {
                newEffect = effectObj.AddComponent<T>();
            }

            return newEffect;
        }

        /// <summary>
        /// 特效回收回调
        /// </summary>
        private void OnEffectRecycle(EffectBase effect)
        {
            // 从活跃列表中移除
            _activeEffects.Remove(effect.InstanceId);
            PoolManager.Instance.PushGameObject(effect.gameObject);
        }
    }
}