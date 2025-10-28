namespace GameLogic.Game
{
    public class SkillWindDownBehavior : SkillBehavior
    {
        public SkillWindDownData Data { get; set; }

        protected override void OnInit()
        {
            base.OnInit();
            StartTime = Data.startTime;
            Duration = Data.duration;
        }

        public override void Clear()
        {
            base.Clear();
            Data = null;
        }
    }
}