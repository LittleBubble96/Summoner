namespace GameLogic.Game
{
    public class SkillAreaBehavior: SkillBehavior
    {
        public AreaData Area { get; set; }

        protected override void OnInit()
        {
            StartTime = Area.startTime;
            Duration = Area.duration;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Clear()
        {
            base.Clear();
            Area = null;
        }
    }
}