using GameConfig.role;
using UnityEngine;

namespace GameLogic.Game
{
    public class AIEntity
    {
        private BehaviorTree m_BehaviorTree;
        private Role m_roleConfig;
        
        public void Init(Role roleConfig)
        {
            m_BehaviorTree = new BehaviorTree();
            BTGenInfo btGenInfo = new BTGenInfo(roleConfig.Ai);
            m_BehaviorTree.Init(btGenInfo);
        }

        public void OnUpdate()
        {
            if (m_BehaviorTree!=null)
            {
                m_BehaviorTree.Execute(Time.deltaTime);
            }
        }
    }
}