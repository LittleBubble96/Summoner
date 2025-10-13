namespace GameLogic.Game
{
    public class CharacterTargetSelection
    {
        public ActorInstanceId TargetId { get; set; }

        public CharacterElement TargetCharacter => CharacterManager.Instance.GetCharacter(TargetId);
    }
}