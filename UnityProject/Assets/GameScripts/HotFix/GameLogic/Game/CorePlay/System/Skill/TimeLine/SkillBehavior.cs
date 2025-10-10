using GameFramework;

namespace GameLogic.Game
{
    public class SkillBehavior : IReference
    {
        public float StartTime { get; set; }
        public float Duration { get; set; }
        public BehaviorState State { get; set; }

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
    }

}