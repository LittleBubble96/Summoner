using System;
using System.Collections.Generic;
using GameFramework;

namespace GameLogic.Game
{
    public class SkillTimeLine : IReference
    {
        public List<SkillTrack> SkillTracks = new List<SkillTrack>();
        private float _currentTime;
        private Action _onSkillComplete;

        public void SetTrack(SkillTrack skillTrack)
        {
            SkillTracks.Add(skillTrack);
        }

        public void SetSkillOnComplete(Action complete)
        {
            _onSkillComplete = complete;
        }

        public bool OnUpdate(float dt)
        {
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
        }

        public void BroadcastSkillComplete()
        {
            _onSkillComplete?.Invoke();
            _onSkillComplete = null;
        }
    }
}