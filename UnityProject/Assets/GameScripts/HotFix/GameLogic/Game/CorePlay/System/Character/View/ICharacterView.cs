namespace GameLogic.Game
{
    public interface ICharacterView
    {
        void OnCreateMainCharacter(MainCharacter character);
        void OnCreateAICharacter(AICharacter character);
        void OnUpdate(float dt);
        void OnDestroyCharacter(ActorInstanceId actorInstanceId);
    }
}