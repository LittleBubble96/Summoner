using GameFramework;

namespace GameLogic.Game
{
    public class BuffConfig
    {
        //模拟
        public int id;
        public string name;
        public string desc;
        public int executeCount;
        public float internalTime;
        public float duration;
        public string param1;
    }

    public abstract class BuffItemBase : IReference
    {
        
        public float ExecuteTime { get; set; } //执行时间
        public float ExecuteCount { get; set; } //执行次数

        public void ExecuteBuff()
        {
            ExecuteCount++;
            ExecuteTime = 0;
            OnExecuteBuff();
        }

        //初始化buff
        public abstract void InitBuff(BuffConfig config,ActorInstanceId belongToActor , ActorInstanceId attachToActor);
        //重复添加buff
        public abstract void RefreshBuff();
        //执行buff
        protected abstract void OnExecuteBuff();
        //清理离开buff
        public abstract void RemoveBuff();
        
        public abstract int GetExecuteCount();
        public abstract float GetInternalTime();
        public abstract float GetDuration();
        public abstract EBuffType GetBuffType();
        
          
        

        public virtual void Clear()
        {
            ExecuteTime = 0;
            ExecuteCount = 0;
        }
    }

    public class BuffItem : BuffItemBase 
    {
        protected BuffConfig Config;
        protected ActorInstanceId BelongToActor;
        protected ActorInstanceId AttachToActor;

        #region 接口
        
        public override void InitBuff(BuffConfig config ,ActorInstanceId belongToActor , ActorInstanceId attachToActor)
        {
            Config = config;
        }

        public override void RefreshBuff()
        {
            
        }

        protected override void OnExecuteBuff()
        {
            
        }

        public override void RemoveBuff()
        {
            
        }

        public override int GetExecuteCount()
        {
            return Config.executeCount;
        }

        public override float GetInternalTime()
        {
            return Config.internalTime;
        }

        public override float GetDuration()
        {
            return Config.duration;
        }

        public override EBuffType GetBuffType()
        {
            return (EBuffType)Config.id;
        }

        #endregion
      
    }
}