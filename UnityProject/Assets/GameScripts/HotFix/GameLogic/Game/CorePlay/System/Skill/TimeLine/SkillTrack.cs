using System.Collections.Generic;
using GameFramework;

namespace GameLogic.Game
{
    public class SkillTrack : IReference
    {
        public List<SkillBehavior> Behaviors = new List<SkillBehavior>();
        
        public virtual void AddBehavior(SkillBehaviorData behaviorData)
        {
            
        }

        protected void AddBehaviorInList(SkillBehavior skillBehavior)
        {
            Behaviors.Add(skillBehavior);
        }

        public bool Update(float dt , float currentTime)
        {
            bool isAllComplete = true;
            for (int i = Behaviors.Count - 1; i >= 0; i--)
            {
                if (Behaviors[i].StartTime <= currentTime && Behaviors[i].State == BehaviorState.Ready)
                {
                    Behaviors[i].State = BehaviorState.Running;
                    Behaviors[i].OnEnter();
                }

                if (Behaviors[i].State == BehaviorState.Running && Behaviors[i].StartTime + Behaviors[i].Duration >= currentTime)
                {
                    Behaviors[i].OnUpdate(dt,currentTime);
                }

                if (Behaviors[i].State == BehaviorState.Running && Behaviors[i].StartTime + Behaviors[i].Duration < currentTime)
                {
                    Behaviors[i].OnExit();
                    Behaviors[i].State = BehaviorState.Complete;
                }

                if (Behaviors[i].State != BehaviorState.Complete)
                {
                    isAllComplete = false;
                }
            }
            return isAllComplete;
        }

        public void Clear()
        {
            foreach (var behavior in Behaviors)
            {
                ReferencePool.Release(behavior);
            }
            Behaviors.Clear();
        }
    }
}