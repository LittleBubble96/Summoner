using UnityGameFramework.Runtime;

namespace GameLogic.Game
{
    public static class XYEvent
    {
        public static EventComponent GEvent => GameSystem.GetComponent<EventComponent>();
    }
}