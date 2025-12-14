using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

/// <summary>
/// 通用资源引用查找工具（修复版，无协程依赖）
/// </summary>
public class UniversalReferenceFinder : EditorWindow
{
    // 目标资源相关
    private string _targetAssetPath;
    private string _targetAssetGuid;
    private Object _targetAsset;

    // 扫描范围
    private string _scanFolderPath = "Assets"; // 默认全项目
    private bool _scanPrefabs = true;          // 扫描预制体
    private bool _scanScenes = true;           // 扫描场景
    private bool _scanOtherAssets = true;      // 扫描其他资源（材质/贴图/脚本等）

    // 引用结果分类存储
    private List<ReferenceItem> _prefabReferences = new List<ReferenceItem>();
    private List<ReferenceItem> _sceneReferences = new List<ReferenceItem>();
    private List<ReferenceItem> _otherReferences = new List<ReferenceItem>();

    // 界面相关
    private Vector2 _scrollPos;
    private bool _isScanning = false;
    private int _scanBatchSize = 50; // 每批扫描50个资源后让出帧，避免卡死

    // 引用项数据结构
    private class ReferenceItem
    {
        public string Path;       // 资源路径
        public Object Asset;      // 资源对象
        public string Type;       // 资源类型
    }

    // ========== 菜单栏入口 ==========
    [MenuItem("Assets/查找所有引用（通用版）", false, 100)]
    private static void OpenFromAssets()
    {
        var window = GetWindow<UniversalReferenceFinder>("通用资源引用查找");
        window.minSize = new Vector2(700, 500);

        // 自动填充选中资源
        if (Selection.activeObject != null)
        {
            window._targetAsset = Selection.activeObject;
            window._targetAssetPath = AssetDatabase.GetAssetPath(window._targetAsset);
            window._targetAssetGuid = AssetDatabase.AssetPathToGUID(window._targetAssetPath);
        }

        window.Show();
    }

    [MenuItem("Window/通用资源引用查找工具")]
    private static void OpenFromWindow()
    {
        var window = GetWindow<UniversalReferenceFinder>("通用资源引用查找");
        window.minSize = new Vector2(700, 500);
        window.Show();
    }

    // ========== 界面绘制 ==========
    private void OnGUI()
    {
        // 防止扫描中操作
        GUI.enabled = !_isScanning;

        // 标题
        GUILayout.Label("通用资源引用查找工具（修复版）", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // 1. 目标资源选择区
        GUILayout.Label("🔍 目标资源", EditorStyles.boldLabel);
        _targetAsset = EditorGUILayout.ObjectField("要查找的资源：", _targetAsset, typeof(Object), false);
        if (_targetAsset != null)
        {
            _targetAssetPath = AssetDatabase.GetAssetPath(_targetAsset);
            _targetAssetGuid = AssetDatabase.AssetPathToGUID(_targetAssetPath);
            GUILayout.Label($"资源路径：{_targetAssetPath}", EditorStyles.miniLabel);
            GUILayout.Label($"资源类型：{_targetAsset.GetType().Name}", EditorStyles.miniLabel);
        }
        GUILayout.Space(10);

        // 2. 扫描范围设置区
        GUILayout.Label("📌 扫描范围", EditorStyles.boldLabel);
        _scanFolderPath = EditorGUILayout.TextField("扫描文件夹：", _scanFolderPath);
        if (GUILayout.Button("选择文件夹", GUILayout.Width(120)))
        {
            string folder = EditorUtility.OpenFolderPanel("选择扫描文件夹", Application.dataPath, "");
            if (!string.IsNullOrEmpty(folder) && folder.StartsWith(Application.dataPath))
            {
                _scanFolderPath = "Assets" + folder.Substring(Application.dataPath.Length);
            }
        }

        _scanPrefabs = EditorGUILayout.Toggle("扫描预制体（.prefab）", _scanPrefabs);
        _scanScenes = EditorGUILayout.Toggle("扫描场景（.unity）", _scanScenes);
        _scanOtherAssets = EditorGUILayout.Toggle("扫描其他资源（材质/贴图/脚本等）", _scanOtherAssets);
        GUILayout.Space(10);

        // 3. 扫描操作区
        GUILayout.BeginHorizontal();
        GUI.enabled = !_isScanning && _targetAsset != null;
        if (GUILayout.Button("开始扫描", GUILayout.Height(30), GUILayout.Width(150)))
        {
            StartScan();
        }
        GUI.enabled = true;

        if (GUILayout.Button("清空结果", GUILayout.Height(30), GUILayout.Width(150)))
        {
            ClearResults();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        // 4. 结果展示区
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        // 预制体引用
        DrawReferenceGroup("预制体引用", _prefabReferences);
        // 场景引用
        DrawReferenceGroup("场景引用", _sceneReferences);
        // 其他资源引用
        DrawReferenceGroup("其他资源引用（材质/贴图等）", _otherReferences);

        EditorGUILayout.EndScrollView();

        // 恢复GUI状态
        if (_isScanning)
        {
            GUI.enabled = true;
        }
    }

    /// <summary>
    /// 绘制单个引用分组
    /// </summary>
    private void DrawReferenceGroup(string title, List<ReferenceItem> items)
    {
        if (items.Count == 0) return;

        GUILayout.Label($"{title}（共 {items.Count} 个）", EditorStyles.boldLabel);
        foreach (var item in items)
        {
            GUILayout.BeginHorizontal("box");
            // 资源路径
            GUILayout.Label($"{item.Type}：{item.Path}", GUILayout.ExpandWidth(true));
            // 定位按钮
            if (GUILayout.Button("定位", GUILayout.Width(60)))
            {
                Selection.activeObject = item.Asset;
                EditorGUIUtility.PingObject(item.Asset);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(10);
    }

    // ========== 核心扫描逻辑 ==========
    private void StartScan()
    {
        // 前置校验
        if (_targetAsset == null)
        {
            EditorUtility.DisplayDialog("错误", "请先选择要查找的目标资源！", "确定");
            return;
        }

        if (!Directory.Exists(Path.Combine(Application.dataPath.Replace("Assets", ""), _scanFolderPath)))
        {
            EditorUtility.DisplayDialog("错误", "扫描文件夹路径无效！", "确定");
            return;
        }

        // 清空历史结果
        ClearResults();

        // 标记扫描中
        _isScanning = true;
        Repaint();

        // 同步扫描（分段处理，避免卡死）
        ScanReferences();

        // 扫描完成
        _isScanning = false;
        Repaint();

        // 扫描完成提示
        string tip = $"扫描完成！\n预制体引用：{_prefabReferences.Count} 个\n场景引用：{_sceneReferences.Count} 个\n其他资源引用：{_otherReferences.Count} 个";
        EditorUtility.DisplayDialog("完成", tip, "确定");
        Debug.Log($"【{_targetAsset.name}】引用汇总：\n{tip}");
    }

    /// <summary>
    /// 分段扫描所有引用（核心修复：移除协程，改用分段让出帧）
    /// </summary>
    private void ScanReferences()
    {
        List<string> scanGuids = new List<string>();
        List<string> filterTypes = new List<string>();

        // 1. 收集要扫描的资源类型
        if (_scanPrefabs) filterTypes.Add("t:Prefab");
        if (_scanScenes) filterTypes.Add("t:Scene");
        if (_scanOtherAssets)
        {
            filterTypes.AddRange(new[] { "t:Material", "t:Texture2D", "t:Sprite", "t:AudioClip", "t:Shader", "t:Script", "t:AnimationClip" });
        }

        // 2. 按类型查找资源GUID（去重）
        HashSet<string> uniqueGuids = new HashSet<string>();
        foreach (var type in filterTypes)
        {
            string[] guids = AssetDatabase.FindAssets(type, new[] { _scanFolderPath });
            foreach (var guid in guids) uniqueGuids.Add(guid);
        }
        // 排除目标资源自身
        uniqueGuids.Remove(_targetAssetGuid);
        scanGuids = new List<string>(uniqueGuids);

        if (scanGuids.Count == 0)
        {
            EditorUtility.DisplayDialog("提示", "未找到符合扫描范围的资源！", "确定");
            return;
        }

        // 3. 分段扫描资源（每N个资源让出帧）
        int total = scanGuids.Count;
        int current = 0;

        try
        {
            foreach (var guid in scanGuids)
            {
                current++;
                float progress = (float)current / total;
                EditorUtility.DisplayProgressBar("扫描引用中", $"正在检查 {current}/{total} 个资源", progress);

                // 每扫描指定数量的资源，让出帧（避免编辑器卡死）
                if (current % _scanBatchSize == 0)
                {
                    EditorUtility.UnloadUnusedAssetsImmediate();
                    System.Threading.Thread.Sleep(10); // 短暂休眠，释放CPU
                    Repaint(); // 刷新窗口，避免假死
                }

                // 检查单个资源引用
                ProcessSingleAsset(guid);
            }
        }
        catch (Exception e)
        {
            EditorUtility.DisplayDialog("扫描异常", $"扫描出错：{e.Message}", "确定");
            Debug.LogError($"资源引用扫描异常：{e}\n{e.StackTrace}");
        }
        finally
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.UnloadUnusedAssetsImmediate(); // 清理内存
        }
    }

    /// <summary>
    /// 处理单个资源的引用检查（移出try-catch的yield问题）
    /// </summary>
    private void ProcessSingleAsset(string guid)
    {
        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
        Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
        if (asset == null || string.IsNullOrEmpty(assetPath)) return;

        // 检查是否引用目标资源
        bool isReferenced = false;
        if (assetPath.EndsWith(".unity"))
        {
            // 场景单独检查（避免try-catch嵌套问题）
            isReferenced = CheckSceneReference(assetPath);
        }
        else
        {
            // 普通资源检查
            isReferenced = CheckAssetSerializedReference(asset);
        }

        // 添加到对应分组
        if (isReferenced)
        {
            AddReferenceItem(asset, assetPath);
        }
    }

    /// <summary>
    /// 检查场景是否引用目标资源
    /// </summary>
    private bool CheckSceneReference(string scenePath)
    {
        bool isReferenced = false;
        Scene tempScene = default;

        // 单独try-catch处理场景加载，避免影响整体扫描
        try
        {
            // 加载场景到内存（不激活）
            tempScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            if (!tempScene.IsValid()) return false;

            // 遍历场景所有根对象
            foreach (var rootObj in tempScene.GetRootGameObjects())
            {
                if (IsGameObjectReferenceTarget(rootObj))
                {
                    isReferenced = true;
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"检查场景 {scenePath} 时出错：{e.Message}");
        }
        finally
        {
            // 关闭临时场景
            if (tempScene.IsValid())
            {
                EditorSceneManager.CloseScene(tempScene, true);
            }
        }

        return isReferenced;
    }

    /// <summary>
    /// 检查GameObject（预制体/场景对象）是否引用目标资源
    /// </summary>
    private bool IsGameObjectReferenceTarget(GameObject go)
    {
        // 检查当前对象组件
        Component[] components = go.GetComponents<Component>();
        foreach (var comp in components)
        {
            if (comp == null) continue;
            if (CheckAssetSerializedReference(comp))
            {
                return true;
            }
        }

        // 递归检查子对象
        foreach (Transform child in go.transform)
        {
            if (IsGameObjectReferenceTarget(child.gameObject))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 通用序列化引用检查（材质/预制体/组件等）
    /// </summary>
    private bool CheckAssetSerializedReference(Object asset)
    {
        SerializedObject so = new SerializedObject(asset);
        SerializedProperty prop = so.GetIterator();

        // 遍历所有序列化字段
        while (prop.Next(true))
        {
            // 检查普通对象引用
            if (prop.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (prop.objectReferenceValue != null)
                {
                    string refGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prop.objectReferenceValue));
                    if (refGuid == _targetAssetGuid)
                    {
                        return true;
                    }
                }
            }
            // 检查数组中的引用（如材质数组）
            else if (prop.isArray && prop.propertyType == SerializedPropertyType.Generic)
            {
                for (int i = 0; i < prop.arraySize; i++)
                {
                    SerializedProperty arrayElement = prop.GetArrayElementAtIndex(i);
                    if (arrayElement.propertyType == SerializedPropertyType.ObjectReference && arrayElement.objectReferenceValue != null)
                    {
                        string refGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(arrayElement.objectReferenceValue));
                        if (refGuid == _targetAssetGuid)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 添加引用项到对应分组
    /// </summary>
    private void AddReferenceItem(Object asset, string assetPath)
    {
        ReferenceItem item = new ReferenceItem
        {
            Path = assetPath,
            Asset = asset,
            Type = asset.GetType().Name
        };

        if (assetPath.EndsWith(".prefab"))
        {
            _prefabReferences.Add(item);
        }
        else if (assetPath.EndsWith(".unity"))
        {
            _sceneReferences.Add(item);
        }
        else
        {
            _otherReferences.Add(item);
        }

        // 输出可跳转日志
        Debug.Log($"找到引用：{assetPath}", asset);
    }

    /// <summary>
    /// 清空所有结果
    /// </summary>
    private void ClearResults()
    {
        _prefabReferences.Clear();
        _sceneReferences.Clear();
        _otherReferences.Clear();
        Repaint();
    }
}