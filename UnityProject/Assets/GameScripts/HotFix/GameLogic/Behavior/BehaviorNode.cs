using System.Collections.Generic;
using GameFramework;

public class BehaviorNode : IReference
{
    protected BehaviorTree behaviorTree;
    protected ConfBTCShape cfgBTItem;
    
    public virtual void AssembleBtNode(BehaviorTree bt, ConfBTCShape cfg)
    {
        this.behaviorTree = bt;
        this.cfgBTItem = cfg;
        this.ParseParams(cfg.StringParams);
    }

    protected virtual void ParseParams(string[] args)
    {
        // Parse parameters from the args array
    }

    public virtual void Begin()
    {
        // Initialize the node
    }

    public virtual void Execute(float deltaTime)
    {
        // Execute the node logic
    }

    public virtual void End()
    {
        // Clean up the node
    }
    
    public virtual void Clear()
    {
        // Recycle the node
        this.cfgBTItem = null;
        this.behaviorTree = null;
    }

    protected List<BehaviorNode> Assemble(BehaviorTree bt, ConfBTCShape[] nodes)
    {
        List<BehaviorNode> nodeList = new List<BehaviorNode>();
        if (nodes == null || nodes.Length == 0)
        {
            return nodeList;
        }
        foreach (var node in nodes)
        {
            BehaviorNode behaviorNode = ReferencePool.Acquire(node.BTNodeType) as BehaviorNode;// ClientFactory.Instance.GetBehaviorNodeFactory().GetObject(node.BTNodeType);
            if (behaviorNode != null)
            {
                behaviorNode.AssembleBtNode(bt, node);
                nodeList.Add(behaviorNode);
            }
        }
        return nodeList;
    }
    
    public virtual bool GetCanExecuteResult()
    {
        // Check if the node can execute
        return true;
    }

    public virtual BtNodeResult RunState()
    {
        return BtNodeResult.None;
    }
}