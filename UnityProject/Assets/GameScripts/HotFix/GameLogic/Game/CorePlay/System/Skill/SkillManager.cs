using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using GameBase;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameLogic.Game
{
    public class SkillManager : BaseLogicSys<SkillManager>
    {
        private List<SkillTimeLine> _skillTimeLines;
        private Dictionary<int, SkillData> _skillDataCache;

        public override bool OnInit()
        {
            _skillTimeLines = new List<SkillTimeLine>();
            _skillDataCache = new Dictionary<int, SkillData>();
            return base.OnInit();
        }

        public void ExecuteSkill(int skillId, Action onComplete)
        {
            SkillData skillData = GetSkillDataById(skillId);
            SkillTimeLine skillTimeLine = ReferencePool.Acquire<SkillTimeLine>();
            skillTimeLine.InitSkillData(skillData);
            skillTimeLine.SetSkillOnComplete(onComplete);
            _skillTimeLines.Add(skillTimeLine);
        }

        private SkillData GetSkillDataById(int skillId)
        {
            if (_skillDataCache.TryGetValue(skillId ,out var skillData))
            {
                return skillData;
            }
            skillData = SkillDataParse.Read(skillId);
            if (skillData == null)
            {
                return null;
            }
            _skillDataCache.Add(skillId, skillData);
            return skillData;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            for (int i = _skillTimeLines.Count - 1; i >= 0; i--)
            {
                bool isComplete = _skillTimeLines[i].OnUpdate(Time.deltaTime);
                if (isComplete)
                {
                    _skillTimeLines[i].BroadcastSkillComplete();
                    RemoveSkillTimeLine(i);
                }
            }
        }

        private void RemoveSkillTimeLine(int index)
        {
            if (index >= _skillTimeLines.Count || index < 0)
            {
                return;
            }
            SkillTimeLine skillTimeLine = _skillTimeLines[index];
            if (skillTimeLine == null)
            {
                return;
            }
            ReferencePool.Release(skillTimeLine);
            _skillTimeLines.RemoveAt(index);
        }
    }
    
}