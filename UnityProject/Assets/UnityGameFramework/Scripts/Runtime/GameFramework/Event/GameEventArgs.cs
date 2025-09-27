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
    
    public class GameEventCustomOneParam<T> : GameEventArgs
    {
        public int CustomId { get; set; }
        public T Param { get; set; }

        public override void Clear()
        {
            CustomId = 0;
        }

        public override int Id { get => CustomId; }
    }
}
