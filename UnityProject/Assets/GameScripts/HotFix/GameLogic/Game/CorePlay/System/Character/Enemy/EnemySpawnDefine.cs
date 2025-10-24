using System;
using GameConfig.enemyLevel;
using UnityEngine;

namespace GameLogic.Game
{
    //怪物生成场景信息
    [Serializable]
    public class EnemySpawnSceneInfo
    {
        public Vector3 ScenePos;
        public Vector3 SceneRot;
        public int EmenySpawnId;

    }

    //所有怪物生成场景信息
    [Serializable]
    public class AllEnemySpawnSceneInfos
    {
        public EnemySpawnSceneInfo[] EnemySpawnSceneInfos;
    }
}