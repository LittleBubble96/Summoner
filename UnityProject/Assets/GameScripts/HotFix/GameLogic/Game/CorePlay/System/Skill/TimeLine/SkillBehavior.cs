using GameFramework;

namespace GameLogic.Game
{
    public class SkillBehavior : IReference
    {
        protected ActorInstanceId OwnerActorInstanceId;
        public float StartTime { get; set; }
        public float Duration { get; set; }
        public BehaviorState State { get; set; }

        public void Init(ActorInstanceId actorInstanceId)
        {
            OwnerActorInstanceId = actorInstanceId;
            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }

        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public virtual void OnUpdate(float dt ,float currentTime)
        {
            
        }

        public virtual void Clear()
        {
            StartTime = 0;
            Duration = 0;
            State = BehaviorState.Ready;
        }

        #region 其他

        //获取角色
        protected CharacterElement GetCharacter()
        {
            CharacterElement character = CharacterManager.Instance.GetCharacter(OwnerActorInstanceId);
            if (character == null)
            {
                return null;
            }
            return character;
        }

        #endregion
    }

}