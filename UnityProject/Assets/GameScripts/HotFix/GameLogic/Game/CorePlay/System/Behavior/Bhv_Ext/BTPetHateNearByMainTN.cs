using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.Game
{
    public class BTPetHateNearByMainTN : BTTaskNode
    {
        //如果找不到的时间间隔 
        private float _findTargetInterval = 1f;
        private float _findTargetTime = 0;

        protected override void OnRecycle()
        {

        }

        protected override void OnBegin()
        {
            
        }

        protected override void OnEnd()
        {

        }

        protected override BtNodeResult OnExecute(float deltaTime)
        {
            _findTargetTime -= deltaTime;
            if (_findTargetTime > 0)
            {
                return BtNodeResult.InProgress;
            }

            _findTargetTime = _findTargetInterval;
            TargetComponent targetComponent = behaviorTree.GetOwnerCharacter().GetComponent<TargetComponent>();
            ActorInstanceId targetId = FindTarget();
            targetComponent.SetTargetActorId(targetId);
            return targetComponent.TargetIsValid() ? BtNodeResult.Succeeded : BtNodeResult.Failed;
        }

        protected override void OnParseParams(string[] args)
        {

        }

        //查找目标
        private ActorInstanceId FindTarget()
        {
            Dictionary<ActorInstanceId, CharacterElement> allCharacter = CharacterManager.Instance.GetAllCharacter();
            ActorInstanceId ownerId = behaviorTree.GetOwnerCharacter().ActorInstanceId;
            float distance = float.MaxValue;
            ActorInstanceId targetId = default;
            CharacterElement mainCharacter = CharacterManager.Instance.MainCharacter();
            foreach (var character in allCharacter)
            {
                if (CharacterManager.Instance.GetRelation(character.Key, ownerId) == FactionRelationType.Hostile &&
                    !character.Value.IsDead())
                {
                    float ds = Vector3.Distance(character.Value.GetPosition(), mainCharacter.GetPosition());
                    if (ds < distance)
                    {
                        targetId = character.Key;
                        distance = ds;
                    }
                }
            }

            return targetId;
        }
    }
}