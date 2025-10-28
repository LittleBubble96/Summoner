using System;
using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;

namespace GameLogic.Game
{
    public class SkillTimeLine : IReference
    {
        public List<SkillTrack> SkillTracks = new List<SkillTrack>();
        private float _currentTime;
        private Action _onSkillComplete;
        private ActorInstanceId _ownerActorInstanceId;
        private float _timeLineScale = 1f;

        public void InitSkillData(SkillData skillData,ActorInstanceId actorInstanceId)
        {
            _ownerActorInstanceId = actorInstanceId;
            CharacterElement character = CharacterManager.Instance.GetCharacter(actorInstanceId);
            if (character != null)
            {
                _timeLineScale = character.AttackSpeed;
            }
            //动画轨道
            foreach (var t in skillData.animationTracks)
            {
                AnimationTrack animationTrack = ReferencePool.Acquire<AnimationTrack>();
                foreach (var animationClip in t.clips)
                {
                    animationTrack.AddBehavior(animationClip);
                }
                SetTrack(animationTrack);
            }
            //发射物轨道
            foreach (var t in skillData.projectileTracks)
            {
                ProjectileTrack projectileTrack = ReferencePool.Acquire<ProjectileTrack>();
                foreach (var projectileClip in t.ProjectileClipDatas)
                {
                    projectileTrack.AddBehavior(projectileClip);
                }
                SetTrack(projectileTrack);
            }
            //前摇轨道
            foreach (var t in skillData.skillWindUpTracks)
            {
                SkillWindUpTrack windUpTrack = ReferencePool.Acquire<SkillWindUpTrack>();
                foreach (var windUpClip in t.clipDatas)
                {
                    windUpTrack.AddBehavior(windUpClip);
                }
                SetTrack(windUpTrack);
            }
            //后摇轨道
            foreach (var t in skillData.skillWindDownTracks)
            {
                SkillWindDownTrack windDownTrack = ReferencePool.Acquire<SkillWindDownTrack>();
                foreach (var windDownClip in t.clipDatas)
                {
                    windDownTrack.AddBehavior(windDownClip);
                }
                SetTrack(windDownTrack);
            }
        }

        private void SetTrack(SkillTrack skillTrack)
        {
            skillTrack.Init(_ownerActorInstanceId);
            SkillTracks.Add(skillTrack);
        }

        public void SetSkillOnComplete(Action complete)
        {
            _onSkillComplete = complete;
        }

        public bool OnUpdate(float dt)
        {
            dt *= _timeLineScale;
            bool isAllExecuteComplete = true;
            for (int i = SkillTracks.Count - 1; i >= 0; i--)
            {
                if (!SkillTracks[i].Update(dt,_currentTime))
                {
                    isAllExecuteComplete = false;
                }
            }

            _currentTime += dt;
            return isAllExecuteComplete;
        }

        public void Clear()
        {
            foreach (var skillTrack in SkillTracks)
            {
                ReferencePool.Release(skillTrack);
            }
            SkillTracks.Clear();
            _currentTime = 0;
            _onSkillComplete = null;
            _timeLineScale = 1f;
        }

        public void BroadcastSkillComplete()
        {
            Log.Info($"当前timeLine技能播放完成，广播技能完成事件，执行时长:{_currentTime}");
            _onSkillComplete?.Invoke();
            _onSkillComplete = null;
        }
    }
}