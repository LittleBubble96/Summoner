using UnityEngine;

namespace GameLogic.Game
{
    public interface IBuildingView
    {
        void OnGenerateGrid();
        void OnCreateBuilding(BuildingItem item,Vector3 pos);
    }
}