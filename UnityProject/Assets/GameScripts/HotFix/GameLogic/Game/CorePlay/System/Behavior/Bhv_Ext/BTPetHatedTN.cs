using System.Collections.Generic;
using GameLogic.Game;
using UnityEngine;

public class BTPetHatedTN : BTTaskNode
{
    //如果找不到的时间间隔 
    private float findTargetInterval = 1f;
    private float findTargetTime = 0;
    protected override void OnRecycle()
    {
        
    }

    protected override void OnBegin()
    {
        // TargetComponent targetComponent = behaviorTree.GetAIController().TryOrAddActorComponent<TargetComponent>();
        // findTargetTime = findTargetInterval;
        // //toDO 先 直接设置为主角
        // Actor actor = GetTargetActor();
        // if (actor == null)
        // {
        //     return;
        // }
        // targetComponent.SetTargetActorId(actor.GetActorId());
    }

    protected override void OnEnd()
    {
        
    }

    protected override BtNodeResult OnExecute(float deltaTime)
    {
        findTargetTime -= deltaTime;
        if (findTargetTime > 0)
        {
            return BtNodeResult.InProgress;
        }
        findTargetTime = findTargetInterval;
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
        foreach (var character in allCharacter)
        {
            if (CharacterManager.Instance.GetRelation(character.Key,ownerId) == FactionRelationType.Hostile && !character.Value.IsDead())
            {
                float ds = Vector3.Distance(character.Value.GetPosition(), behaviorTree.GetOwnerCharacter().GetPosition());
                if (ds < distance)
                {
                    targetId = character.Key;
                }
            }
        }
        return targetId;
    }
    
}