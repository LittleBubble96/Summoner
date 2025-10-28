using GameLogic.Game;
using UnityEngine;

public class BTPetPreBhvTN : BTTaskNode
{
    private bool findSucc = false;
    protected override void OnRecycle()
    {
        findSucc = false;
    }

    protected override void OnBegin()
    {
        //获取目标
        TargetComponent targetComponent = behaviorTree.GetOwnerCharacter().GetComponent<TargetComponent>();
        if (targetComponent == null)
        {
            findSucc = false;
            return;
        }
        
        if (!targetComponent.TargetIsValid())
        {
            findSucc = false;
            return;
        }
        findSucc = true;
    }

    protected override void OnEnd()
    {
        
    }

    protected override BtNodeResult OnExecute(float deltaTime)
    {
        if (!findSucc)
        {
            return BtNodeResult.Failed;
        }
        TargetComponent targetComponent = behaviorTree.GetOwnerCharacter().GetComponent<TargetComponent>();
        if (targetComponent == null)
        {
            return BtNodeResult.Failed;
        }
        if (!targetComponent.TargetIsValid())
        {
            return BtNodeResult.Failed;
        }
        behaviorTree.GetOwnerCharacter().NavToTarget(targetComponent.GetTargetCharacter().GetPosition());
        //如果距离小于停止距离，则停止
         if (behaviorTree.GetOwnerCharacter().NavToTargetRemainDistance < behaviorTree.GetOwnerCharacter().AttackDistance)
         {
             behaviorTree.GetOwnerCharacter().NavToStop();
             return BtNodeResult.Succeeded;
         }
        return BtNodeResult.InProgress;
    }

    protected override void OnParseParams(string[] args)
    {
        
    }
}