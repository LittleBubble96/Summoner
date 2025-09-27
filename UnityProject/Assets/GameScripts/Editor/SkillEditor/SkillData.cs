using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillData
{
    public string skillName;
    public float duration;
    public List<AnimationTrackData> animationTracks = new List<AnimationTrackData>();
    public List<EffectTrackData> effectTracks = new List<EffectTrackData>();
    public List<BuffTrackData> buffTracks = new List<BuffTrackData>();
    public List<SummonTrackData> summonTracks = new List<SummonTrackData>();
}

[Serializable]
public class TrackDataBase
{
    public string trackName;
}

[Serializable]
public class AnimationTrackData : TrackDataBase
{
    public List<AnimationClipData> clips = new List<AnimationClipData>();
}

[Serializable]
public class AnimationClipData
{
    public string clipName;
    public float startTime;
    public float duration;
    public float speed = 1f;
    public float weight = 1f;
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
    public float[] parameters;
    public bool isPermanent;
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
