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
        // TargetComponent targetComponent = behaviorTree.GetAIController().TryOrAddActorComponent<TargetComponent>();
        // if (targetComponent == null)
        // {
        //     findSucc = false;
        //     return;
        // }
        //
        // if (targetComponent.TargetActorId<=0)
        // {
        //     findSucc = false;
        //     return;
        // }
        // Actor actor = RoomManager.Instance.GetActorById(targetComponent.TargetActorId);
        // if (actor== null)
        // {
        //     findSucc = false;
        //     return;
        // }
        // findSucc = true;
        //
        // behaviorTree.GetAIController().AgentStart();
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
        // TargetComponent targetComponent = behaviorTree.GetAIController().GetActorComponent<TargetComponent>();
        // if (targetComponent == null)
        // {
        //     return BtNodeResult.Failed;
        // }
        // if (targetComponent.TargetActorId<=0)
        // {
        //     return BtNodeResult.Failed;
        // }
        // Actor actor = RoomManager.Instance.GetActorById(targetComponent.TargetActorId);
        // if (actor== null)
        // {
        //     return BtNodeResult.Failed;
        // }
        // behaviorTree.GetAIController().SetTargetPosition(actor.GetPosition());
        // float distance = Vector3.Distance(behaviorTree.GetAIController().transform.position, actor.GetPosition());
        // //如果距离小于停止距离，则停止
        //  if (distance < behaviorTree.GetAIController().GetAttackDistance())
        //  {
        //      behaviorTree.GetAIController().AgentStop();
        //      return BtNodeResult.Succeeded;
        //  }
        return BtNodeResult.InProgress;
    }

    protected override void OnParseParams(string[] args)
    {
        
    }
}