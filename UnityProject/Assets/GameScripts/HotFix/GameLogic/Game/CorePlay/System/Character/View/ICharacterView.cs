namespace GameLogic.Game
{
    public interface ICharacterView
    {
        ICharacterItemView OnCreateMainCharacter(MainCharacter character);
        ICharacterItemView OnCreateAICharacter(AICharacter character);
        void OnUpdate(float dt);
        void OnDestroyCharacter(ActorInstanceId actorInstanceId);
    }
}