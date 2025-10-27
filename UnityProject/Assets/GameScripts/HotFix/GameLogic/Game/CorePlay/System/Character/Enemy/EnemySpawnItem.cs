using GameConfig.enemyLevel;
using GameFramework;
using GameProto;

namespace GameLogic.Game
{
    public class EnemySpawnItem : IReference
    {
        public EnemySpawnSceneInfo SpawnInfo { get; set; }
        public EnemyLevelConfig EnemyLevelConfig { get; set; }
        private float _timerCount;

        public void Init(EnemySpawnSceneInfo spawnInfo)
        {
            SpawnInfo = spawnInfo;
            EnemyLevelConfig = ConfigSystem.Instance.Tables.TbEnemyLevel.Get(spawnInfo.EmenySpawnId);
            _timerCount = 0f;
        }

        public void DoUpdate(float dt)
        {
            if (SpawnInfo == null || EnemyLevelConfig == null)
            {
                return;
            }
            _timerCount += dt * 1000 ;
            if (_timerCount > EnemyLevelConfig.InternalTime)
            {
                int totalWeight = 0;
                for (int i = 0; i < EnemyLevelConfig.EmemyWeight.Length; i++)
                {
                    totalWeight += EnemyLevelConfig.EmemyWeight[i];
                }

                int randValue = Utility.Random.GetRandom(totalWeight);
                int currentWeight = 0;
                for (int i = 0; i < EnemyLevelConfig.EmemyWeight.Length; i++)
                {
                    currentWeight += EnemyLevelConfig.EmemyWeight[i];
                    if (randValue < currentWeight)
                    {
                        CharacterManager.EnemyManager.SpawnEnemy(this,EnemyLevelConfig.EmemyIds[i], SpawnInfo.ScenePos, SpawnInfo.SceneRot);
                        break;
                    }
                }
                _timerCount = int.MinValue;
            }
        }

        public void Clear()
        {
            SpawnInfo = null;
            EnemyLevelConfig = null;
            _timerCount = 0f;
        }
    }
}