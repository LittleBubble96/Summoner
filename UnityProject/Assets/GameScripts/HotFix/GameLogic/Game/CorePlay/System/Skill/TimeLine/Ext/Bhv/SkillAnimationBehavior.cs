namespace GameLogic.Game
{
    public class SkillAnimationBehavior : SkillBehavior
    {
        public AnimationClipData AnimationData { get; set; }

        public override void OnEnter()
        {
            base.OnEnter();
            //TODO 播放角色动画
        }
        
    }

}