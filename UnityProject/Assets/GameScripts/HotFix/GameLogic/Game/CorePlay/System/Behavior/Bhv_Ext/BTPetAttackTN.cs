using GameLogic.Game;
using GameLogic.Game.Common;
using UnityEngine;

public class BTPetAttackTN : BTTaskNode
{
    private bool m_isInSkilling;
    protected override void OnRecycle()
    {
        m_isInSkilling = false;
    }

    protected override void OnBegin()
    {
        m_isInSkilling = false;
        SkillManager.Instance.ExecuteSkill(behaviorTree.GetOwnerCharacter().RoleConfig.NormalSkillId,behaviorTree.GetOwnerCharacter().ActorInstanceId, () =>
        {
            m_isInSkilling = false;
        });
    }

    protected override void OnEnd()
    {
        
    }

    protected override BtNodeResult OnExecute(float deltaTime)
    {
        if (m_isInSkilling)
        {
            return BtNodeResult.InProgress;
        }
        return BtNodeResult.Succeeded;
    }

    protected override void OnParseParams(string[] args)
    {
        
    }
}