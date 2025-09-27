
using System.Collections.Generic;
using GameBase;
using UnityEngine;

namespace GameLogic.Game
{
    public partial class BuildingSystem : BaseLogicSys<BuildingSystem>
    {
        //网格数据
        private GridData m_GridData;
        public GridData GridData => m_GridData;
        // 表现层
        public IBuildingView BuildingView { get; set; }

        // 建筑物数据        
        public List<BuildingItem> BuildingItems { get; set; } = new List<BuildingItem>();
        
        //当前选择得土地
        public int CurrentSelectHexIndex { get; set; } = -1;

        public override bool OnInit()
        {
            m_GridData = new GridData(100, 100,1);
            return base.OnInit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            for (int i = BuildingItems.Count - 1; i >= 0; i--)
            {
                BuildingItems[i].OnUpdate(Time.deltaTime);
            }
        }

        #region 建筑物

        public void CreateBuildingItem(int hexIndex ,Vector3 pos)
        {
            BuildingItem item = new BarracksBuildingItem();
            item.Init(hexIndex);
            BuildingItems.Add(item);
            BuildingView?.OnCreateBuilding(item,pos);
        }

        #endregion

        #region 网格
        
        //设置当前选择得网格
        public void SetSelectHex(int hexIndex)
        {
            CurrentSelectHexIndex = hexIndex;
        }

        public void GenerateGrid()
        {
            // 计算六边形尺寸
            float hexWidth = Mathf.Sqrt(3) * m_GridData.HexSize;   // 对边距离
            float hexHeight = 2f * m_GridData.HexSize;             // 对角距离
        
            // 正确的交错布局参数
            float horizontalSpacing = hexWidth; // 水平间距
            float verticalSpacing = hexHeight * 0.5f;   // 垂直间距
        
            // 计算网格总尺寸
            float totalWidth = (m_GridData.GridWidth - 1) * horizontalSpacing + (m_GridData.GridHeight % 2 == 0 ? horizontalSpacing * 0.5f : 0);
            float totalHeight = (m_GridData.GridHeight - 1) * verticalSpacing * 1.5f;
        
            // 计算偏移量使网格中心位于原点
            float offsetX = -totalWidth / 2f;
            float offsetZ = -totalHeight / 2f;
            int hexIndex = 0;
            int hexCount = m_GridData.GridWidth * m_GridData.GridHeight;
            Vector3[] centers = new Vector3[hexCount];
            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            for (int row = 0; row < m_GridData.GridHeight; row++)
            {
                for (int col = 0; col < m_GridData.GridWidth; col++)
                {
                    // 计算中心位置（交错布局）
                    float xOffset = (row % 2 == 1) ? horizontalSpacing * 0.5f : 0;
                    float xPos = col * horizontalSpacing + xOffset + offsetX;
                    float zPos = row * verticalSpacing * 1.5f + offsetZ; // 修正垂直间距

                    Vector3 center = new Vector3(xPos, 0, zPos);
                    centers[hexIndex] = center;
                    // 生成单个六边形线框（使用共享顶点）
                    CreateHexagonWireframe(center, hexIndex,vertices,indices);
                    hexIndex++;
                    m_GridData.Cells[col, row] = new HexCell
                    {
                        q = col,
                        r = row,
                        worldPos = center
                    };
                }
            }
            m_GridData.HexCenters = centers;
            m_GridData.HexVertices = vertices;
            m_GridData.HexNeighbors = indices;
            BuildingView?.OnGenerateGrid();
        }
        
        private void CreateHexagonWireframe(Vector3 center, int hexIndex,List<Vector3> vertices,List<int> indices)
        {
            // 六边形六个顶点（尖顶布局）
            Vector3[] corners = new Vector3[6];
            for (int i = 0; i < 6; i++)
            {
                float angle = 60f * i - 30f;
                float x = center.x + m_GridData.HexSize * Mathf.Cos(angle * Mathf.Deg2Rad);
                float z = center.z + m_GridData.HexSize * Mathf.Sin(angle * Mathf.Deg2Rad);
                corners[i] = new Vector3(x, 0, z);
                // 添加顶点（每个六边形共享顶点）
                vertices.Add(corners[i]);
            }

            // 添加边索引（6条边）
            int startVertexIndex = hexIndex * 6;
            for (int i = 0; i < 6; i++)
            {
                int next = (i + 1) % 6;
            
                // 添加线段的两个顶点索引
                indices.Add(startVertexIndex + i);
                indices.Add(startVertexIndex + next);
            }
        }
        
        public (bool,Vector3,int) GetHexCoordFromTriangle(int triangleIndex)
        {
            // 每个六边形有6个三角形
            int hexIndex = triangleIndex / 6;
        
            if (hexIndex >= 0 && hexIndex < m_GridData.HexCenters.Length)
            {
                return (true, m_GridData.HexCenters[hexIndex],hexIndex);
            }
            return (false, Vector3.zero,hexIndex);
        }

        #endregion

        //注入
        public void Inject(IBuildingView view)
        {
            BuildingView = view;
        }
    }
}