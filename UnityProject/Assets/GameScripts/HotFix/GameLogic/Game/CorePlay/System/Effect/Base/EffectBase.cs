using UnityEngine;

namespace GameLogic.Game
{
    /// <summary>
    /// 特效基类
    /// 每个实例拥有唯一ID
    /// </summary>
    public abstract class EffectBase : MonoBehaviour
    {
        // 每个特效实例的唯一ID
        public EffectInstanceId InstanceId { get; private set; }
    
        protected float _autoRecycleTime;
        protected float _currentLifeTime;
        protected bool _isActive;
        protected System.Action<EffectInstanceId> _onRecycleCallback;

        /// <summary>
        /// 初始化特效
        /// </summary>
        public virtual void Initialize(float autoRecycleTime, System.Action<EffectInstanceId> onRecycleCallback)
        {
            // 为每个实例分配唯一ID
            InstanceId = EffectInstanceId.NewId();
            _autoRecycleTime = autoRecycleTime;
            _currentLifeTime = 0;
            _isActive = true;
            _onRecycleCallback = onRecycleCallback;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 更新逻辑
        /// </summary>
        public virtual void OnUpdate(float deltaTime)
        {
            if (!_isActive) return;

            // 自动回收逻辑（基类统一处理）
            _currentLifeTime += deltaTime;
            if (_autoRecycleTime > 0 && _currentLifeTime >= _autoRecycleTime)
            {
                Recycle();
            }
        }

        /// <summary>
        /// 回收特效
        /// </summary>
        public virtual void Recycle()
        {
            if (!_isActive) return;

            _isActive = false;
            gameObject.SetActive(false);
            _onRecycleCallback?.Invoke(InstanceId);
        }
    }

}