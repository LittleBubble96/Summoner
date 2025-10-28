namespace GameLogic.Game
{
    public class SkillAnimationBehavior : SkillBehavior
    {
        public AnimationClipData AnimationData { get; set; }

        protected override void OnInit()
        {
            StartTime = AnimationData.startTime;
            Duration = AnimationData.duration;
        }

        public override void OnEnter()
        {
            CharacterElement character = GetCharacter();
            if (character !=null && !character.IsDead())
            {
                character.SetAnimationBool(AnimationData.clipName,true);
            }
        }

        public override void OnExit()
        {
            CharacterElement character = GetCharacter();
            if (character !=null && !character.IsDead())
            {
                character.SetAnimationBool(AnimationData.clipName,false);
            }
        }
    }

}