namespace GameLogic.Game
{
    public class SkillWindDownBehavior : SkillBehavior
    {
        public SkillWindDownData Data { get; set; }

        public override void Clear()
        {
            base.Clear();
            Data = null;
        }
    }
}