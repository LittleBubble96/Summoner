using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillData
{
    public float duration;
    //动画数据
    public List<AnimationTrackData> animationTracks = new List<AnimationTrackData>();
    //发射物数据
    public List<ProjectileTrackData> projectileTracks = new List<ProjectileTrackData>();
    //前摇数据
    public List<SkillWindUpTrackData> skillWindUpTracks = new List<SkillWindUpTrackData>();
    //后摇数据
    public List<SkillWindDownTrackData> skillWindDownTracks = new List<SkillWindDownTrackData>();
}

[Serializable]
public class TrackDataBase
{
    public string trackName;
}

[Serializable]
public class SkillBehaviorData
{
    
}

[Serializable]
public class AnimationTrackData : TrackDataBase
{
    public List<AnimationClipData> clips = new List<AnimationClipData>();
}

[Serializable]
public class AnimationClipData : SkillBehaviorData
{
    public string clipName;
    public float startTime;
    public float duration;
    public float speed = 1f;
    public bool loop;
}

[Serializable]
public class EffectTrackData : TrackDataBase
{
    public List<EffectClipData> effects = new List<EffectClipData>();
}

[Serializable]
public class EffectClipData
{
    public string effectPrefabPath;
    public float startTime;
    public float duration;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;
    public float scale = 1f;
    public bool followTarget;
    public string attachPoint;
}

[Serializable]
public class BuffTrackData : TrackDataBase
{
    public List<BuffClipData> buffs = new List<BuffClipData>();
}

[Serializable]
public class BuffClipData
{
    public int buffId;
    public float startTime;
    public float duration;
}

[Serializable]
public class SummonTrackData : TrackDataBase
{
    public List<SummonClipData> summons = new List<SummonClipData>();
}

[Serializable]
public class SummonClipData
{
    public string summonPrefabPath;
    public float startTime;
    public Vector3 position;
    public Quaternion rotation;
    public float lifetime;
    public string behaviorType;
    public float[] behaviorParameters;
}

// 发射物数据
[Serializable]
public class ProjectileTrackData : TrackDataBase
{
    public List<ProjectileClipData> ProjectileClipDatas = new List<ProjectileClipData>();
}

[Serializable]
public class ProjectileClipData : SkillBehaviorData
{
    public int projectileId;
    public float startTime;
    public Vector3 position;
    public Vector3 rotation;
    public float duration;
}

//前摇数据
[Serializable]
public class SkillWindUpTrackData : TrackDataBase
{
    public List<SkillWindUpData> clipDatas = new List<SkillWindUpData>();
}

[Serializable]
public class SkillWindUpData : SkillBehaviorData
{
    public float startTime;
    public float duration;
}

//后摇数据
[Serializable]
public class SkillWindDownTrackData : TrackDataBase
{
    public List<SkillWindDownData> clipDatas = new List<SkillWindDownData>();
}

[Serializable]
public class SkillWindDownData : SkillBehaviorData
{
    public float startTime;
    public float duration;
}