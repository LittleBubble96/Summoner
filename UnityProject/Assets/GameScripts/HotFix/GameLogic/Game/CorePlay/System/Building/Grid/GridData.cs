using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.Game
{
    public class GridData
    {
        public float HexSize; // 六边形边长
        public int GridWidth;      // 网格宽度
        public int GridHeight;     // 网格高度
        public Color LineColor; // 网格线颜色
        public Vector3[] HexCenters; // 六边形中心位置数组
        public List<Vector3> HexVertices; // 六边形顶点位置数组
        public List<int> HexNeighbors; // 六边形邻居索引数组
        public HexCell[,] Cells;
        
        public GridData(int width, int height, float hexSize)
        {
            GridWidth = width;
            GridHeight = height;
            HexSize = hexSize;
            LineColor = Color.white;
            HexCenters = null;
            HexVertices = null;
            HexNeighbors = null;
            Cells = new HexCell[width, height];
        }
        
        /// <summary>
        /// 把世界坐标转成 offset 坐标 (col,row)
        /// 适配 3D-Y 朝上，六边形尖顶布局
        /// </summary>
        public bool WorldToHex(Vector3 world, out int col, out int row)
        {
            // 尖顶布局公式（Unity 左手坐标系）
            float qf = (Mathf.Sqrt(3) / 3f * world.x - 1f / 3f * world.z) / HexSize;
            float rf = (2f / 3f * world.z) / HexSize;

            // 轴向坐标 -> offset 坐标（odd-r）
            col = Mathf.RoundToInt(qf + (Mathf.RoundToInt(rf) & 1) * 0.5f);
            row = Mathf.RoundToInt(rf);

            return col >= 0 && col < GridWidth && row >= 0 && row < GridHeight;
        }
    }
    
    public class HexCell
    {
        public Vector3 worldPos;
        public int q;              // 横向坐标
        public int r;               // 轴向坐标
        public bool walkable = true;
        public int moveCost = 1;       // 额外消耗
        public HexCell parent;         // A* 用
        public int gCost, hCost;
        public int FCost => gCost + hCost;
    }
}