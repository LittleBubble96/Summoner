using System;
using GameFramework;
using UnityEngine;

namespace GameLogic.Game
{
    public class BuildingViewRoot : MonoBehaviour , IBuildingView
    {
        [SerializeField] private HexGridGenerator m_HexGridGenerator;
        [SerializeField] private SelectGridGenerator m_SelectGridGenerator;
        [SerializeField] private Transform m_BuildingParent;
        
        private float _dragTimeCount;
        private void Awake()
        {
            BuildingSystem.Instance.Inject(this);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnCheckMouse();
            }
            if (Input.GetMouseButton(0))
            {
                _dragTimeCount += Time.deltaTime;
                if (_dragTimeCount > 0.5f)
                {
                    OnCheckMouse();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                _dragTimeCount = 0;
                OnCheckMouse();
            }
        }
        
        private void OnCheckMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000, LayerDefine.Grid))
            {
                (bool ,Vector3,int) result = BuildingSystem.Instance.GetHexCoordFromTriangle(hitInfo.triangleIndex);
                Vector3 pos = result.Item2 + hitInfo.transform.position + new Vector3(0, 0, 0);
                if (result.Item1 && result.Item3 != BuildingSystem.Instance.CurrentSelectHexIndex)
                {
                    BuildingSystem.Instance.SetSelectHex(result.Item3);
                    ChangeSelectGrid(result.Item3,pos);
                }
            }
        }

        #region 接口
        public void OnGenerateGrid()
        {
            m_HexGridGenerator.GenerateHexGrid();
            m_SelectGridGenerator.GenerateSelectedGrid();
        }

        public void OnCreateBuilding(BuildingItem item ,Vector3 pos)
        {
            var building = GameModule.Resource.LoadGameObject("building1", parent: m_BuildingParent);
            building.transform.position = pos;
        }
        
        #endregion
        
        public void ChangeSelectGrid(int hexIndex,Vector3 pos)
        {
            if (hexIndex < 0)
            {
                m_SelectGridGenerator.SetVisible(false);
                return;
            }
            m_SelectGridGenerator.SetPosition(pos);
            m_SelectGridGenerator.SetVisible(true);
        }

       
    }
}