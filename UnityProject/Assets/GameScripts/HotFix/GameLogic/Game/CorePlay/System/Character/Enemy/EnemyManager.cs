using System.Collections.Generic;
using GameBase;
using GameFramework;
using GameProto;
using UnityEngine;

namespace GameLogic.Game
{
    public class EnemyManager
    {
        private List<EnemySpawnItem> _enemySpawnItems = new List<EnemySpawnItem>();
        private List<ActorInstanceId> _enemyInstanceIds = new List<ActorInstanceId>();

        public bool IsSpawning { get; set; } = false;

        //开始生成敌人
        public void StartSpawn()
        {
            _enemySpawnItems.Clear();
            AllEnemySpawnSceneInfos allEnemySpawnSceneInfos = ReadEnemySpawnSceneInfos();
            foreach (var enemySpawnScene in allEnemySpawnSceneInfos.EnemySpawnSceneInfos)
            {
                EnemySpawnItem enemySpawnItem = ReferencePool.Acquire<EnemySpawnItem>();
                enemySpawnItem.Init(enemySpawnScene);
                _enemySpawnItems.Add(enemySpawnItem);
            }

            IsSpawning = true;
        }

        //停止生成敌人
        public void StopSpawn()
        {
            IsSpawning = false;
            _enemySpawnItems.Clear();
        }

        public void OnUpdate()
        {
            foreach (var enemySpawnItem in _enemySpawnItems)
            {
                enemySpawnItem.DoUpdate(Time.deltaTime);
            }
        }

        //生成敌人
        public void SpawnEnemy(EnemySpawnItem spawnItem ,int enemyId, Vector3 pos, Vector3 rot)
        {
            ActorInstanceId aiCharacter = CharacterManager.Instance.CreateAICharacter(enemyId, pos, rot, CharacterFactionType.Enemy);
            _enemyInstanceIds.Add(aiCharacter);
        }
        

        //获取敌人生成场景信息
        //TODO 临时写法，后续需要改成从配置表读取
        private AllEnemySpawnSceneInfos ReadEnemySpawnSceneInfos()
        {
            AllEnemySpawnSceneInfos _enemySpawnSceneInfos = new AllEnemySpawnSceneInfos();
            _enemySpawnSceneInfos.EnemySpawnSceneInfos = new EnemySpawnSceneInfo[0];
            var sceneInfo = new EnemySpawnSceneInfo();
            sceneInfo.ScenePos = new Vector3(-0.5f, 0, 7f);
            sceneInfo.SceneRot = Vector3.zero;
            sceneInfo.EmenySpawnId = 1;
            _enemySpawnSceneInfos.EnemySpawnSceneInfos[0] = new EnemySpawnSceneInfo();
            return _enemySpawnSceneInfos;
        }
    }
}