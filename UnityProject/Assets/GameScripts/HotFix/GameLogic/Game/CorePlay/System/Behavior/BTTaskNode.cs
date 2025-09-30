using System.Collections.Generic;
using GameFramework;

public abstract class BTTaskNode : BehaviorNode
{
    protected BtNodeResult BTNodeRunRes;
    private List<BehaviorNode> Services = new List<BehaviorNode>();
    private List<BehaviorNode> Decorators = new List<BehaviorNode>();
    
    public override void AssembleBtNode(BehaviorTree bt, ConfBTCShape cfg)
    {
        base.AssembleBtNode(bt, cfg);
        this.Decorators = Assemble(bt, cfg.Decorators);
        this.Services = Assemble(bt, cfg.Services);
    }
    
    public override void Clear()
    {
        base.Clear();
        foreach (var service in Services)
        {
            ReferencePool.Release(service);
        }
        foreach (var decorator in Decorators)
        {
            ReferencePool.Release(decorator);
        }
        Services.Clear();
        Decorators.Clear();
        this.BTNodeRunRes = BtNodeResult.None;
    }
    
    public override void Begin()
    {
        base.Begin();
        foreach (var decorator in Decorators)
        {
            decorator.Begin();
        }
        BTNodeRunRes = BtNodeResult.None;
    }
    
    public override void End()
    {
        base.End();
        foreach (var decorator in Decorators)
        {
            decorator.End();
        }
        foreach (var service in Services)
        {
            service.End();
        }
    }
    
    public override void Execute(float deltaTime)
    {
        base.Execute(deltaTime);
        foreach (var decorator in Decorators)
        {
            decorator.Execute(deltaTime);
            if (decorator.GetCanExecuteResult() == false)
            {
                BTNodeRunRes = BtNodeResult.Failed;
                return;
            }
        }
        BtNodeResult lastResult = BTNodeRunRes;
        if (lastResult != BtNodeResult.InProgress)
        {
            this.OnBegin();
        }
        BTNodeRunRes = this.OnExecute(deltaTime);
    }

    public override BtNodeResult RunState()
    {
        return BTNodeRunRes;
    }
    

    protected override void ParseParams(string[] args)
    {
        base.ParseParams(args);
        if (args != null && args.Length > 0)
        {
            OnParseParams(args);
        }
    }
    
    protected abstract void OnRecycle();
    protected abstract void OnBegin();
    protected abstract void OnEnd();
    protected abstract BtNodeResult OnExecute(float deltaTime);
    protected abstract void OnParseParams(string[] args);

   
}