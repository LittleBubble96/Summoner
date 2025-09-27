using UnityEngine;

namespace GameLogic.Game
{
    public class FixedEffect: EffectBase
    {
        /// <summary>
        /// 初始化固定特效
        /// </summary>
        public void Initialize(Vector3 position, Quaternion rotation, Transform parent, 
            float autoRecycleTime, System.Action<EffectBase> onRecycleCallback)
        {
            // 设置位置和旋转
            transform.position = position;
            transform.rotation = rotation;
            transform.parent = parent;
        
            // 调用基类初始化
            base.Initialize(autoRecycleTime, onRecycleCallback);
        }
    }
}