using UnityEngine;

namespace GameLogic.Game
{
    public static class LayerDefine
    {
        //6
        public static LayerMask Grid = 1 << 6;
        //子弹射线检测
        public static LayerMask LayerProjectileHit = (1 << 7) | (1 << 8);
    }
}