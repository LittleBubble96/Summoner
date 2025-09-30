namespace GameLogic.Game
{
    public class BarracksBuildingItem : BuildingItem
    {
        private float _spawnTimer = 0;
        private float _spawnTimeCount;

        protected override void OnInit()
        {
            _spawnTimer = 5f;
            _spawnTimeCount = _spawnTimer;
        }

        public override void OnUpdate(float t)
        {
            if (_spawnTimeCount > 0)
            {
                _spawnTimeCount -= t;
                if (_spawnTimeCount <= 0)
                {
                    _spawnTimeCount = _spawnTimer;
                    // CharacterManager.Instance.CreateCharacter(HexIndex);
                }
            }
        }
    }
}