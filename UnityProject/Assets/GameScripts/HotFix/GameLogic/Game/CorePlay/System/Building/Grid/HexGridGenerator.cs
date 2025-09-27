// HexGridGenerator.cs
using UnityEngine;
using System.Collections.Generic;

namespace GameLogic.Game
{
    public class HexGridGenerator : MonoBehaviour
    {
        public Material gridMaterial;  // 线框材质

        private Mesh wireframeMesh;
        private Mesh collisionMesh;
        private GameObject collisionObject;

        private GridData GridData => BuildingSystem.Instance.GridData;

        public void GenerateHexGrid()
        {
            // 创建线框网格
            wireframeMesh = new Mesh();
            GetComponent<MeshFilter>().mesh = wireframeMesh;
            // 设置线框网格数据
            wireframeMesh.vertices = GridData.HexVertices.ToArray();
            int[] indices = GridData.HexNeighbors.ToArray();
            wireframeMesh.SetIndices(indices, MeshTopology.Lines, 0);
            wireframeMesh.RecalculateBounds();
            
            // 创建碰撞网格
            CreateCollisionMesh();
            
            // 设置材质
            GetComponent<MeshRenderer>().material = gridMaterial;
        }

        void CreateCollisionMesh()
        {
            // 创建碰撞专用游戏对象
            collisionObject = new GameObject("HexCollision");
            collisionObject.transform.SetParent(transform);
            collisionObject.transform.localPosition = Vector3.zero;
            collisionObject.layer = gameObject.layer;

            // 创建碰撞网格
            collisionMesh = new Mesh();
            List<Vector3> collisionVerts = new List<Vector3>();
            List<int> collisionTris = new List<int>();
            Vector3[] hexVertices = GridData.HexCenters;
            for (int i = 0; i < hexVertices.Length; i++)
            {
                Vector3 center = hexVertices[i];
                // 生成六边形填充面
                Vector3[] corners = new Vector3[6];
                for (int j = 0; j < 6; j++)
                {
                    float angle = 60f * j - 30f;
                    float x = center.x + GridData.HexSize * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float z = center.z + GridData.HexSize * Mathf.Sin(angle * Mathf.Deg2Rad);
                    corners[j] = new Vector3(x, 0, z);
                }
                
                int startIndex = collisionVerts.Count;
                collisionVerts.AddRange(corners);
                
                // 组成三角形（每个六边形6个三角形）
                for (int j = 0; j < 6; j++)
                {
                    collisionTris.Add(startIndex + (j + 2) % 6);
                    collisionTris.Add(startIndex + (j + 1) % 6);
                    collisionTris.Add(startIndex);
                }
            }
            
            collisionMesh.vertices = collisionVerts.ToArray();
            collisionMesh.triangles = collisionTris.ToArray();
            collisionMesh.RecalculateBounds();
            collisionMesh.RecalculateNormals();

            // 添加碰撞组件
            MeshCollider collider = collisionObject.AddComponent<MeshCollider>();
            collider.sharedMesh = collisionMesh;
        }
    }
}
