namespace GameLogic.Game
{
    public static class DamageIdGenerator
    {
        private static uint _damageId = 0;

        public static uint NewId()
        {
            return _damageId++;
        }
    }
}