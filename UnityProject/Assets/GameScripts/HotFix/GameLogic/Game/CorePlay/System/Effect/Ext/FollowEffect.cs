using UnityEngine;

namespace GameLogic.Game
{
    public class FollowEffect : EffectBase
    {
        private Transform _target;
        private Vector3 _offset;

        /// <summary>
        /// 初始化跟随特效
        /// </summary>
        public void Initialize(Transform target, Vector3 offset, Quaternion rotation,
            float autoRecycleTime, System.Action<EffectBase> onRecycleCallback)
        {
            _target = target;
            _offset = offset;
            transform.rotation = rotation;
        
            // 初始位置设置
            if (_target != null)
            {
                transform.position = _target.position + _offset;
            }
        
            // 调用基类初始化
            base.Initialize(autoRecycleTime, onRecycleCallback);
        }

        /// <summary>
        /// 更新跟随位置
        /// </summary>
        public override void OnUpdate(float deltaTime)
        {
            // 先执行基类的更新逻辑（时间和自动回收）
            base.OnUpdate(deltaTime);
        
            if (!_isActive || _target == null) return;
        
            // 更新位置（跟随目标）
            transform.position = _target.position + _offset;
        }

        /// <summary>
        /// 回收时清理目标引用
        /// </summary>
        public override void Recycle()
        {
            _target = null;
            base.Recycle();
        }
    }

}