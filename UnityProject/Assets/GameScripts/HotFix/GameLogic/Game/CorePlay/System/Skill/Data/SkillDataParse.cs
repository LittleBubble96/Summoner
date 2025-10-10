using System.IO;
using Config;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameLogic.Game
{
    public class SkillDataParse
    {
        private const string SkillDataSavePath = "Assets/AssetRaw/Configs/Skills/";

        public static SkillData Read(int skillId)
        {
            //加载技能
            string skillPath = $"{SkillDataSavePath}Skill_{skillId}.bytes";
            TextAsset skillText = GameModule.Resource.LoadAsset<TextAsset>(skillPath);
            if (skillText == null)
            {
                Log.Error($"没有找到对应的技能配置:{skillPath}");
                return null;
            }
            SkillData skillData = new SkillData();
            using (ByteArray byteArray = new ByteArray(skillText.bytes))
            {
                skillData.duration = byteArray.ReadFloat();
                //读取动画轨道
                int trackCount = byteArray.ReadInt();
                for (int i = 0; i < trackCount; i++)
                {
                    AnimationTrackData trackData = new AnimationTrackData();
                    int clipCount = byteArray.ReadInt();
                    for (int j = 0; j < clipCount; j++)
                    {
                        AnimationClipData clipData = new AnimationClipData();
                        clipData.clipName = byteArray.ReadString();
                        clipData.startTime = byteArray.ReadFloat();
                        clipData.duration = byteArray.ReadFloat();
                        clipData.speed = byteArray.ReadFloat();
                        clipData.loop = byteArray.ReadBool();
                        trackData.clips.Add(clipData);
                    }
                    skillData.animationTracks.Add(trackData);
                }
                //读取召唤物轨道
                int projectileTrackCount = byteArray.ReadInt();
                for (int i = 0; i < projectileTrackCount; i++)
                {
                    ProjectileTrackData projectileTrackData = new ProjectileTrackData();
                    int projectileClipCount = byteArray.ReadInt();
                    for (int j = 0; j < projectileClipCount; j++)
                    {
                        ProjectileClipData projectileClipData = new ProjectileClipData();
                        projectileClipData.projectileId = byteArray.ReadInt();
                        projectileClipData.startTime = byteArray.ReadFloat();
                        projectileClipData.duration = byteArray.ReadFloat();
                        byteArray.ReadVector3(out var posX, out var posY, out var posZ);
                        projectileClipData.position = new Vector3(posX, posY, posZ);
                        byteArray.ReadVector3(out var rotX, out var rotY, out var rotZ);
                        projectileClipData.rotation = new Vector3(rotX, rotY, rotZ);
                        projectileTrackData.ProjectileClipDatas.Add(projectileClipData);
                    }
                    skillData.projectileTracks.Add(projectileTrackData);
                }
            }
            return skillData;
        }

        public static void Write(int skillId , SkillData skillData)
        {
            using (ByteArray byteArray = new ByteArray())
            {
                byteArray.WriteFloat(skillData.duration);
                //写入动画轨道
                byteArray.WriteInt(skillData.animationTracks.Count);
                for (int i = 0; i < skillData.animationTracks.Count; i++)
                {
                    AnimationTrackData trackData = skillData.animationTracks[i];
                    byteArray.WriteInt(trackData.clips.Count);
                    for (int j = 0; j < trackData.clips.Count; j++)
                    {
                        AnimationClipData clipData = trackData.clips[j];
                        byteArray.WriteString(clipData.clipName);
                        byteArray.WriteFloat(clipData.startTime);
                        byteArray.WriteFloat(clipData.duration);
                        byteArray.WriteFloat(clipData.speed);
                        byteArray.WriteBool(clipData.loop);
                    }
                }
                //写入召唤物轨道
                byteArray.WriteInt(skillData.projectileTracks.Count);
                foreach (var projectileTrack in skillData.projectileTracks)
                {
                    byteArray.WriteInt(projectileTrack.ProjectileClipDatas.Count);
                    foreach (var projectileClip in projectileTrack.ProjectileClipDatas)
                    {
                        byteArray.WriteInt(projectileClip.projectileId);
                        byteArray.WriteFloat(projectileClip.startTime);
                        byteArray.WriteFloat(projectileClip.duration);
                        byteArray.WriteVector3(projectileClip.position.x, projectileClip.position.y, projectileClip.position.z);
                        byteArray.WriteVector3(projectileClip.rotation.x, projectileClip.rotation.y, projectileClip.rotation.z);
                    }
                }
                string fullPath = Path.Combine(Application.dataPath, "../", SkillDataSavePath, $"Skill_{skillId}.bytes");
                File.WriteAllBytes(fullPath, byteArray.Bytes);
                Log.Info("Skill data exported successfully to: " + fullPath);
            }
        }
    }
}