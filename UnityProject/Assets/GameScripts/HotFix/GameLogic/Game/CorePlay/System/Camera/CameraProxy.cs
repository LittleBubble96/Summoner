using GameFramework;
using UnityEngine;

namespace GameLogic.Game
{
    public class CameraProxy : IReference
    {
        public Camera CameraInstance { get; set; }
        private CameraBehavior m_Behavior;

        public virtual void Init(Camera camera)
        {
            CameraInstance = camera;
        }

        public virtual void OnUpdate(float dt)
        {
            if (m_Behavior != null)
            {
                m_Behavior.OnExecute(CameraInstance, dt);
            }
        }

        public virtual void Clear()
        {
            CameraInstance = null;    
            if (m_Behavior != null)
            {
                ReferencePool.Release(m_Behavior);
                m_Behavior = null;
            }
        }
        
        public void SetBehaviorType(CameraBehaviorType cameraBehaviorType)
        {
            if (m_Behavior != null)
            {
                ReferencePool.Release(m_Behavior);
                m_Behavior = null;
            }

            switch (cameraBehaviorType)
            {
                case CameraBehaviorType.FixedPosition:
                    m_Behavior = ReferencePool.Acquire<CameraFixedPositionBehavior>();
                    break;
                case CameraBehaviorType.FollowTargetImmediate:
                    m_Behavior = ReferencePool.Acquire<CameraFollowTargetImmediateBehavior>();
                    break;
                case CameraBehaviorType.FollowTargetSmooth:
                    m_Behavior = ReferencePool.Acquire<CameraFollowTargetSmoothBehavior>();
                    break;
                default:
                    break;
            }
        }
        
        public void SetBehaviorArgs(CommonArgs args)
        {
            m_Behavior?.OnSetup(args);
            ReferencePool.Release(args);
        }
    }
}