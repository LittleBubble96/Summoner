using GameFramework;

namespace GameLogic.Game
{
    public class ActorComponent : IReference
    {
        public ActorInstanceId OwnerId;
        public void Init(ActorInstanceId ownerId)
        {
            OwnerId = ownerId;
        }

        public virtual void RegisterEvent()
        {
            
        }

        public virtual void UnRegisterEvent()
        {
            
        }

        public virtual void DoUpdate(float dt)
        {
            
        }

        public virtual void Clear()
        {
            OwnerId = default;
        }
    }
}