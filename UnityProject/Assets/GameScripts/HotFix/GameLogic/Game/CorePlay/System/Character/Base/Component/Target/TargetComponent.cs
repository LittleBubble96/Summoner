using System.Collections.Generic;
using GameFramework;

namespace GameLogic.Game
{
    public class TargetComponent : ActorComponent
    {
        private ActorInstanceId m_TargetActorInstanceId;
        
        public ActorInstanceId TargetActorInstanceId => m_TargetActorInstanceId;
        
        public void SetTargetActorId(ActorInstanceId targetActorInstanceId)
        {
            m_TargetActorInstanceId = targetActorInstanceId;
        }
        
        public bool TargetIsValid()
        {
            if (!m_TargetActorInstanceId.IsValid())
            {
                return false;
            }
            CharacterElement targetCharacter = CharacterManager.Instance.GetCharacter(m_TargetActorInstanceId);
            if (targetCharacter == null || targetCharacter.IsDead())
            {
                return false;
            }
            return true;
        }

        public CharacterElement GetTargetCharacter()
        {
            return CharacterManager.Instance.GetCharacter(m_TargetActorInstanceId);
        }
    }
}