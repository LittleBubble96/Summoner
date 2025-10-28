using GameFramework;
using UnityEngine;

namespace GameLogic.Game
{
    public class CameraBehavior : IReference
    {
        public virtual void OnSetup(CommonArgs args)
        {
            
        }
        
        public virtual void OnExecute(Camera camera, float dt)
        {
            
        }

        public virtual void Clear()
        {
            
        }
    }
    
    public enum CameraBehaviorType
    {
        FixedPosition = 0,
        FollowTargetImmediate = 1,
        FollowTargetSmooth = 2,
    }
}