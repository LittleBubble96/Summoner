// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// [Serializable]
// public class SkillData
// {
//     public float duration;
//     public List<AnimationTrackData> animationTracks = new List<AnimationTrackData>();
//     public List<ProjectileTrackData> projectileTracks = new List<ProjectileTrackData>();
// }
//
// [Serializable]
// public class TrackDataBase
// {
//     public string trackName;
// }
//
// [Serializable]
// public class AnimationTrackData : TrackDataBase
// {
//     public List<AnimationClipData> clips = new List<AnimationClipData>();
// }
//
// [Serializable]
// public class AnimationClipData
// {
//     public string clipName;
//     public float startTime;
//     public float duration;
//     public float speed = 1f;
//     public bool loop;
// }
//
// [Serializable]
// public class EffectTrackData : TrackDataBase
// {
//     public List<EffectClipData> effects = new List<EffectClipData>();
// }
//
// [Serializable]
// public class EffectClipData
// {
//     public string effectPrefabPath;
//     public float startTime;
//     public float duration;
//     public Vector3 positionOffset;
//     public Quaternion rotationOffset;
//     public float scale = 1f;
//     public bool followTarget;
//     public string attachPoint;
// }
//
// [Serializable]
// public class BuffTrackData : TrackDataBase
// {
//     public List<BuffClipData> buffs = new List<BuffClipData>();
// }
//
// [Serializable]
// public class BuffClipData
// {
//     public int buffId;
//     public float startTime;
//     public float duration;
// }
//
// [Serializable]
// public class SummonTrackData : TrackDataBase
// {
//     public List<SummonClipData> summons = new List<SummonClipData>();
// }
//
// [Serializable]
// public class SummonClipData
// {
//     public string summonPrefabPath;
//     public float startTime;
//     public Vector3 position;
//     public Quaternion rotation;
//     public float lifetime;
//     public string behaviorType;
//     public float[] behaviorParameters;
// }
//
//
// [Serializable]
// public class ProjectileTrackData : TrackDataBase
// {
//     public List<ProjectileClipData> ProjectileClipDatas = new List<ProjectileClipData>();
// }
//
// [Serializable]
// public class ProjectileClipData
// {
//     public int projectileId;
//     public float startTime;
//     public Vector3 position;
//     public Vector3 rotation;
//     public float duration;
// }