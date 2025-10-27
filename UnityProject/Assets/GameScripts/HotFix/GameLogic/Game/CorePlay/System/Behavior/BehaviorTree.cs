using GameFramework;
using GameLogic.Game;
using UnityEngine;

public class BehaviorTree
{
    private BlackBoard blackBoard;
    
    private BehaviorNode rootNode;
    
    private BTGenInfo btGenInfo;
    
    private BtNodeResult btRootRunRes;
    
    private AICharacter ownerCharacter;
    
    
    public void Init(BTGenInfo info , AICharacter aiCharacter)
    {
        this.btGenInfo = info;
        this.blackBoard = new BlackBoard();
        this.rootNode = InitTree();
        this.ownerCharacter = aiCharacter;
    }
    
    public void Recycle()
    {
        this.blackBoard.Clear();
        this.blackBoard = null;
        if (this.rootNode != null)
        {
            ReferencePool.Release(this.rootNode);
            this.rootNode = null;
        }
        this.btGenInfo = default;
        this.btRootRunRes = BtNodeResult.None;
    }

    public BlackBoard GetBlackBoard()
    {
        return this.blackBoard;
    }

    public BTGenInfo GetBTGenInfo()
    {
        return this.btGenInfo;
    }

    public AICharacter GetOwnerCharacter()
    {
        return this.ownerCharacter;
    }

    private BehaviorNode InitTree()
    {
        ConfBTCShape cfgBtData = BTConfigHelper.Instance.GetConfBtcShape(btGenInfo.GetBtGenId());
        if (cfgBtData == null)
        {
            Debug.LogError($"BehaviorTree InitTree: cfgBTData is null, btGenId: {btGenInfo.GetBtGenId()}");
            return null;
        }

        BehaviorNode root =ReferencePool.Acquire(cfgBtData.BTNodeType) as BehaviorNode;
        if (root != null)
        {
            root.AssembleBtNode(this, cfgBtData);
            return root;
        }
        return null;
    }


    public void Execute(float deltaTime)
    {
        if (this.rootNode == null)
        {
            return;
        }

        if (btRootRunRes != BtNodeResult.InProgress)
        {
            rootNode.Begin();
        }
        rootNode.Execute(deltaTime);
        btRootRunRes = rootNode.RunState();
        switch (btRootRunRes)
        {
            case BtNodeResult.Succeeded:
                case BtNodeResult.Failed:
                rootNode.End();
                break;
            case BtNodeResult.InProgress:
                break;
            default:
                this.btRootRunRes = BtNodeResult.Failed;
                break;
        }
    }

}

/**
 * 行为树自定义数据
 */
public enum BtNodeResult
{
    None = 0,
    Succeeded = 1,
    Failed,
    InProgress,
}