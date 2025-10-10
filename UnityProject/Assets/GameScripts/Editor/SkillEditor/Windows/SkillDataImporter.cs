using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public static class SkillDataImporter
{
    public static SkillData LoadSkillData(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("Skill data file not found: " + filePath);
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Open))
        {
            try
            {
                return formatter.Deserialize(stream) as SkillData;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load skill data: " + e.Message);
                return null;
            }
        }
    }

    public static void ApplySkillData(PlayableDirector director, SkillData skillData)
    {
        if (director == null || skillData == null) return;

        // 创建临时Timeline
        TimelineAsset timeline = ScriptableObject.CreateInstance<TimelineAsset>();
        //设置时间线的总时长
        timeline.fixedDuration = skillData.duration;

        // 添加动画轨道
        foreach (var animTrackData in skillData.animationTracks)
        {
            AnimationParameterTrack animTrack = timeline.CreateTrack<AnimationParameterTrack>(null, animTrackData.trackName);
            
            foreach (var clipData in animTrackData.clips)
            {
                AnimationClip clip = Resources.Load<AnimationClip>(clipData.clipName);
                if (clip != null)
                {
                    AnimationParameterClip animClip = ScriptableObject.CreateInstance<AnimationParameterClip>();
                    animClip.template.animationClip = clip;
                    animClip.template.speed = clipData.speed;
                    animClip.template.loop = clipData.loop;
                    TimelineClip timelineClip = animTrack.CreateClip<AnimationParameterClip>();
                    timelineClip.asset = animClip;
                    timelineClip.start = clipData.startTime;
                    timelineClip.duration = clipData.duration;
                }
            }
        }

        // 类似地添加其他类型的轨道...

        // 播放技能
        director.playableAsset = timeline;
        director.Play();
    }
}
