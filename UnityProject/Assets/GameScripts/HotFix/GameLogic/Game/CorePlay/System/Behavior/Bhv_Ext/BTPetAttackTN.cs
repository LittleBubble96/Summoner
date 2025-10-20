using GameLogic.Game;
using UnityEngine;

public class BTPetAttackTN : BTTaskNode
{
    private bool m_isInSkilling;
    protected override void OnRecycle()
    {
       
    }

    protected override void OnBegin()
    {
        m_isInSkilling = true;
        SkillManager.Instance.ExecuteSkill(behaviorTree.GetOwnerCharacter().RoleConfig.NormalSkillId, () =>
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