namespace GameLogic.Game
{
    public class BuildingItem
    {
        public int HexIndex { get; set; }
        
        public void Init(int hexIndex)
        {
            HexIndex = hexIndex;
            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }

        public virtual void OnUpdate(float t)
        {
            
        }
    }
}