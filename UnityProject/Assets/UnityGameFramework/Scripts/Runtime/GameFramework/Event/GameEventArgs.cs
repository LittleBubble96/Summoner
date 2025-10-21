namespace GameFramework.Event
{
    /// <summary>
    /// 游戏逻辑事件基类。
    /// </summary>
    public abstract class GameEventArgs : BaseEventArgs
    {
    }
    
    public class GameEventCustom : GameEventArgs
    {
        public int CustomId { get; set; }

        public override void Clear()
        {
            CustomId = 0;
        }

        public override int Id { get => CustomId; }
    }
    
    public class GameEventCustomOneParam<T> : GameEventCustom
    {
        public T Param { get; set; }

        public override void Clear()
        {
            base.Clear();
            Param = default(T);
        }
    }
    
    public class GameEventCustomTwoParam<T,T1> : GameEventCustomOneParam<T>
    {
        public T1 Param1 { get; set; }
        public override void Clear()
        {
            base.Clear();
            Param1 = default(T1);
        }
    }
    
    public class GameEventCustomThreeParam<T,T1,T2> : GameEventCustomTwoParam<T,T1>
    {
        public T2 Param2 { get; set; }
        public override void Clear()
        {
            base.Clear();
            Param2 = default(T2);
        }
    }
}
