using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.Game
{
    public partial class BuildingSystem
    {
        #region A* 寻路扩展

        /// <summary>
        /// 寻找从 startWorld → endWorld 的六边形 A* 路径
        /// 返回世界坐标列表（已包含起点和终点），失败返回 null
        /// </summary>
        public List<Vector3> FindPath(Vector3 startWorld, Vector3 endWorld)
        {
            // 1. 世界坐标 → offset 坐标
            if (!m_GridData.WorldToHex(startWorld, out int sc, out int sr)) return null;
            if (!m_GridData.WorldToHex(endWorld,   out int ec, out int er)) return null;

            HexCell startCell = m_GridData.Cells[sc, sr];
            HexCell endCell   = m_GridData.Cells[ec, er];

            if (!startCell.walkable || !endCell.walkable) return null;

            // 2. 开放/关闭列表
            List<HexCell> open   = new List<HexCell> { startCell };
            HashSet<HexCell> close = new HashSet<HexCell>();

            startCell.gCost = 0;
            startCell.hCost = HexDistance(startCell, endCell);

            // 3. 6 方向邻居
            Vector2Int[] dirs = (sr & 1) == 1
                ? new Vector2Int[] { new Vector2Int(1,0), new Vector2Int(0,-1), new Vector2Int(-1,-1),
                                     new Vector2Int(-1,0), new Vector2Int(-1,1), new Vector2Int(0,1) }
                : new Vector2Int[] { new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(0,-1),
                                     new Vector2Int(-1,0), new Vector2Int(0,1),  new Vector2Int(1,1) };

            while (open.Count > 0)
            {
                HexCell cur = open[0];
                for (int i = 1; i < open.Count; ++i)
                    if (open[i].FCost < cur.FCost || (open[i].FCost == cur.FCost && open[i].hCost < cur.hCost))
                        cur = open[i];

                open.Remove(cur);
                close.Add(cur);

                if (cur == endCell)
                    return RetracePath(startCell, endCell);

                foreach (var d in dirs)
                {
                    int nc = cur.q + d.x;
                    int nr = cur.r + d.y;
                    if (nc < 0 || nc >= m_GridData.GridWidth || nr < 0 || nr >= m_GridData.GridHeight) continue;

                    HexCell neighbour = m_GridData.Cells[nc, nr];
                    if (!neighbour.walkable || close.Contains(neighbour)) continue;

                    int newGCost = cur.gCost + neighbour.moveCost;
                    if (newGCost < neighbour.gCost || !open.Contains(neighbour))
                    {
                        neighbour.gCost = newGCost;
                        neighbour.hCost = HexDistance(neighbour, endCell);
                        neighbour.parent = cur;
                        if (!open.Contains(neighbour)) open.Add(neighbour);
                    }
                }
            }
            return null;
        }

        /// <summary>六边形曼哈顿距离</summary>
        private int HexDistance(HexCell a, HexCell b)
        {
            return (Mathf.Abs(a.q - b.q)
                  + Mathf.Abs(a.q + a.r - b.q - b.r)
                  + Mathf.Abs(a.r - b.r)) / 2;
        }

        /// <summary>回溯路径</summary>
        private List<Vector3> RetracePath(HexCell start, HexCell end)
        {
            List<Vector3> path = new List<Vector3>();
            HexCell cur = end;
            while (cur != null)
            {
                path.Add(cur.worldPos);
                cur = cur.parent;
            }
            path.Reverse();
            return path;
        }

#endregion
    }
}