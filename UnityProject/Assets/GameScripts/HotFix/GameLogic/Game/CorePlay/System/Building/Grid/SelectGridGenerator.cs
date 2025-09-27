using UnityEngine;

namespace GameLogic.Game
{
    public class SelectGridGenerator : MonoBehaviour
    {
         [Header("Materials")]
        [SerializeField] private Material fillMaterial;   // 填充面
        [SerializeField] private Material borderMaterial; // 高亮边框

        private GridData GridData => BuildingSystem.Instance.GridData;

        private Mesh fillMesh;
        private Mesh borderMesh;

        private GameObject fillGO;
        private GameObject borderGO;

        public void GenerateSelectedGrid()
        {
            // 创建子物体
            if (fillGO == null)
            {
                fillGO = new GameObject("Fill");
                fillGO.transform.SetParent(transform, false);
                fillGO.AddComponent<MeshFilter>();
                MeshRenderer fill = fillGO.AddComponent<MeshRenderer>();
                fill.receiveShadows = false;
                fill.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            if (borderGO == null)
            {
                borderGO = new GameObject("Border");
                borderGO.transform.SetParent(transform, false);
                borderGO.AddComponent<MeshFilter>();
                MeshRenderer border = borderGO.AddComponent<MeshRenderer>();
                border.receiveShadows = false;
                border.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            float hexSize = GridData.HexSize;

            // === 填充六边形 ===
            fillMesh = new Mesh();
            Vector3[] vertices = new Vector3[7];
            int[] triangles = new int[18];

            vertices[0] = Vector3.zero;
            for (int i = 0; i < 6; i++)
            {
                float angle = 60f * i - 30f;
                float x = hexSize * Mathf.Cos(angle * Mathf.Deg2Rad);
                float z = hexSize * Mathf.Sin(angle * Mathf.Deg2Rad);
                vertices[i + 1] = new Vector3(x, 0, z);

                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = (i + 1) % 6 + 1;
                triangles[i * 3 + 2] = i + 1;
            }

            fillMesh.vertices = vertices;
            fillMesh.triangles = triangles;
            fillMesh.RecalculateNormals();
            fillMesh.RecalculateBounds();

            fillGO.GetComponent<MeshFilter>().mesh = fillMesh;
            fillGO.GetComponent<MeshRenderer>().material = fillMaterial;

            // === 边框线 ===
            borderMesh = new Mesh();
            Vector3[] borderVerts = new Vector3[6];
            int[] borderIndices = new int[12]; // 6 条边 * 2

            for (int i = 0; i < 6; i++)
            {
                borderVerts[i] = vertices[i + 1];
                borderIndices[i * 2] = i;
                borderIndices[i * 2 + 1] = (i + 1) % 6;
            }

            borderMesh.vertices = borderVerts;
            borderMesh.SetIndices(borderIndices, MeshTopology.Lines, 0);
            borderMesh.RecalculateBounds();

            borderGO.GetComponent<MeshFilter>().mesh = borderMesh;
            borderGO.GetComponent<MeshRenderer>().material = borderMaterial;

            SetVisible(false);
        }
        
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}