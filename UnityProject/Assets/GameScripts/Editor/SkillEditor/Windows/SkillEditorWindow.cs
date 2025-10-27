using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine.Timeline;
using GameLogic.Game;
using UnityEngine.Playables;

public class SkillEditorWindow : EditorWindow
{
    private TimelineAsset timelineAsset;
    private int skillName = 1;
    private int characterId = 1001;
    private PlayableDirector playableDirector;

    private Transform roleParent;
    private Transform RoleParent
    {
        get
        {
            if (!roleParent)
            {
                roleParent = GameObject.Find("RoleParent")?.transform;
                if (!roleParent)
                {
                    roleParent = new GameObject("RoleParent").transform;
                }
            }
            return roleParent;
        }
    }

    [MenuItem("Window/技能编辑器")]
    public static void ShowWindow()
    {
        GetWindow<SkillEditorWindow>("技能编辑器");
    }

    private void OnGUI()
    {
        GUILayout.Label("技能设置", EditorStyles.boldLabel);
        
        skillName = EditorGUILayout.IntField("技能名", skillName);
        characterId = EditorGUILayout.IntField("角色ID", characterId);
        timelineAsset = EditorGUILayout.ObjectField("Timeline Asset", timelineAsset, typeof(TimelineAsset), false) as TimelineAsset;
        playableDirector = EditorGUILayout.ObjectField("Playable Director", playableDirector, typeof(PlayableDirector), true) as PlayableDirector;

        GUILayout.Space(20);
        
        if (GUILayout.Button("Open Timeline Editor"))
        {
            OpenTimeLine();
        }

        GUILayout.Space(10);
        
        if (GUILayout.Button("Create New Timeline"))
        {
            CreateNewTimeline();
        }

        GUILayout.Space(20);
        GUILayout.Label("Export Settings", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Export Skill Data"))
        {
            if (timelineAsset == null)
            {
                Debug.LogWarning("Please assign a Timeline Asset first!");
                return;
            }
            
            ExportSkillData();
        }

        GUILayout.Space(10);
        
        if (GUILayout.Button("Play Skill"))
        {
            if (playableDirector != null && timelineAsset != null)
            {
                playableDirector.Play(timelineAsset);
            }
            else
            {
                Debug.LogWarning("Please assign both Timeline Asset and Playable Director!");
            }
        }
    }

    private void CreateNewTimeline()
    {
        string path = EditorUtility.SaveFilePanelInProject("Create Timeline", "SkillTimeline", "playable", "Create a new timeline asset");
        if (!string.IsNullOrEmpty(path))
        {
            timelineAsset = TimelineAsset.CreateInstance<TimelineAsset>();
            AssetDatabase.CreateAsset(timelineAsset, path);
            AssetDatabase.SaveAssets();
            
            // 添加默认轨道
            timelineAsset.CreateTrack<AnimationParameterTrack>(null, "Animations");
            timelineAsset.CreateTrack<EffectTrack>(null, "Effects");
            timelineAsset.CreateTrack<BuffTrack>(null, "Buffs");
            timelineAsset.CreateTrack<SummonTrack>(null, "Summons");
        }
    }

    private void ExportSkillData()
    {
        SkillData skillData = new SkillData();
        skillData.duration = (float)timelineAsset.duration;

        // 收集所有轨道数据
        foreach (var track in timelineAsset.GetOutputTracks())
        {
            if (track is AnimationParameterTrack animTrack)
            {
                AnimationTrackData animData = new AnimationTrackData();
                animData.trackName = track.name;
                
                foreach (var clip in track.GetClips())
                {
                    AnimationParameterClip animClip = clip.asset as AnimationParameterClip;
                    if (animClip != null)
                    {
                        AnimationClipData clipData = new AnimationClipData();
                        clipData.clipName = animClip.animationClip != null ? animClip.animationClip.name : "";
                        clipData.startTime = (float)clip.start;
                        clipData.duration = (float)clip.duration;
                        clipData.speed = animClip.template.speed;
                        clipData.loop = animClip.template.loop;
                        
                        animData.clips.Add(clipData);
                    }
                }
                
                skillData.animationTracks.Add(animData);
            }
            else if (track is ProjectileParameterTrack projectileParameterTrack)
            {
                ProjectileTrackData projectileTrackData = new ProjectileTrackData();
                projectileTrackData.trackName = track.name;
                
                foreach (var clip in track.GetClips())
                {
                    ProjectileParameterClip projectileParameterClip = clip.asset as ProjectileParameterClip;
                    if (projectileParameterClip != null)
                    {
                        ProjectileClipData clipData = new ProjectileClipData();
                        clipData.startTime = (float)clip.start;
                        clipData.duration = (float)clip.duration;
                        clipData.projectileId = projectileParameterClip.projectileId;
                        clipData.position = projectileParameterClip.position;
                        clipData.rotation = projectileParameterClip.rotation;
                        projectileTrackData.ProjectileClipDatas.Add(clipData);
                    }
                }
                skillData.projectileTracks.Add(projectileTrackData);
            }
            else if (track is WindUpParameterTrack windUpParameterTrack)
            {
                SkillWindUpTrackData trackData = new SkillWindUpTrackData();
                trackData.trackName = track.name;
                
                foreach (var clip in track.GetClips())
                {
                    WindUpParameterClip clipViewData = clip.asset as WindUpParameterClip;
                    if (clipViewData != null)
                    {
                        SkillWindUpData clipData = new SkillWindUpData();
                        clipData.startTime = (float)clip.start;
                        clipData.duration = (float)clip.duration;
                        trackData.clipDatas.Add(clipData);
                    }
                }
                skillData.skillWindUpTracks.Add(trackData);
            }
            else if (track is WindDownParameterTrack parameterTrack)
            {
                SkillWindDownTrackData trackData = new SkillWindDownTrackData();
                trackData.trackName = track.name;
                
                foreach (var clip in track.GetClips())
                {
                    WindDownParameterClip clipViewData = clip.asset as WindDownParameterClip;
                    if (clipViewData != null)
                    {
                        SkillWindDownData clipData = new SkillWindDownData();
                        clipData.startTime = (float)clip.start;
                        clipData.duration = (float)clip.duration;
                        trackData.clipDatas.Add(clipData);
                    }
                }
                skillData.skillWindDownTracks.Add(trackData);
            }
        }
        SkillDataParse.Write(skillName,skillData);
        AssetDatabase.Refresh();
    }

    private void OpenTimeLine()
    {
        ReadTimeLineAsset();
        ReadCharacterResInTime();
        string fullPath = SkillDataParse.GetSkillDataPath(skillName);
        SkillData skillData = SkillDataParse.ReadByLocalFile(skillName);

    }

    private string GetTimeLineResPath()
    {
        return $"Assets/Skill/res/Skill_{skillName}.playable";
    }
    
    private string GetCharacterResPath()
    {
        return $"Assets/AssetRaw/Actor/Monster/{characterId}/{characterId}.prefab";
    }

    //读取资源的timeLine
    private void ReadTimeLineAsset()
    {
        TimelineAsset timeline = AssetDatabase.LoadAssetAtPath<TimelineAsset>(GetTimeLineResPath());
        if (timeline == null)
        {
            Debug.LogError("未找到对应的Timeline资源，路径：" + GetTimeLineResPath());
            return;
        }

        if (playableDirector == null)
        {
            playableDirector = FindObjectOfType<PlayableDirector>();
        }
        timelineAsset = timeline;
        TimelineEditorWindow window = EditorWindow.GetWindow<TimelineEditorWindow>();
        window.Show();
        window.titleContent = new GUIContent($"技能编辑器 - Skill_{skillName}");
        window.SetTimeline(timelineAsset);
        if (playableDirector != null)
        {
            playableDirector.playableAsset = timelineAsset;
        }
        Selection.activeObject = playableDirector;
    }

    //读取角色资源并赋值给timeLine
    private void ReadCharacterResInTime()
    {
        ClearParent();
        GameObject actorObj = AssetDatabase.LoadAssetAtPath<GameObject>(GetCharacterResPath());
        if (actorObj == null)
        {
            Debug.LogError("未找到对应的角色资源，路径：" + GetCharacterResPath());
            return;
        }
        actorObj = PrefabUtility.InstantiatePrefab(actorObj ,RoleParent) as GameObject;
        if (actorObj == null)
        {
            Debug.LogError("角色资源实例化失败，路径：" + GetCharacterResPath());
            return;
        }
        Animator animator = actorObj.GetComponentInChildren<Animator>();
        playableDirector.SetGenericBinding(timelineAsset.GetOutputTrack(0), animator);
    }

    private void ClearParent()
    {
        for (int i = RoleParent.childCount - 1; i >= 0; i--)
        {
            Transform child = RoleParent.GetChild(i);
            if (child)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
    }
}
