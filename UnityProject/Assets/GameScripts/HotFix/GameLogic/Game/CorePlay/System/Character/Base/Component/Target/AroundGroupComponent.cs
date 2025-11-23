using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace GameLogic.Game
{
    public class AroundGroupComponent : ActorComponent
    {
        private float _range;
        
        private Dictionary<ActorInstanceId,AroundElement> _aroundElements;


        public void Init(float range)
        {
            _range = range;
            _aroundElements = new Dictionary<ActorInstanceId, AroundElement>();
        }

        public void AddAround(ActorInstanceId actorInstanceId)
        {
            if (_aroundElements.ContainsKey(actorInstanceId))
            {
                return;
            }
            AroundElement aroundElement = ReferencePool.Acquire<AroundElement>();
            _aroundElements.Add(actorInstanceId,aroundElement);
            aroundElement.SetTarget(_aroundElements.Count - 1 , _aroundElements.Count , _range);
        }

        public override void RegisterEvent()
        {
            
        }

        public override void UnRegisterEvent()
        {
            
        }

        public override void DoUpdate(float dt)
        {
            foreach (var around in _aroundElements)
            {
                around.Value.DoUpdate(dt);
            }
        }
    }

    public class AroundElement : IReference
    {
        private Vector3 _targetPos;

        private Vector3 _startPos;

        private Vector2? _curPos;

        private float _timer;

        private readonly float _lTime = 1f;
        
        public void SetTarget(int index , int count , float range)
        {
            _targetPos = CalTargetPos(index, count, range);
            _timer = _lTime;
            if (_curPos == null)
            {
                _curPos = _targetPos;
                _startPos = _targetPos;
            }
        }

        public void DoUpdate(float dt)
        {
            if (_timer > 0)
            {
                _timer -= dt;
                float t = 1 - _timer / _lTime;
                _curPos = Vector3.Lerp(_startPos, _targetPos, t);
            }
            else
            {
                _curPos = _targetPos;
            }
        }

        public void Clear()
        {
            _targetPos = Vector3.zero;
            _curPos = null;
            _timer = 0;
        }

        private Vector3 CalTargetPos(int index , int count , float range)
        {
            if (count <= 0)
            {
                return Vector3.right * range;
            }
            return Quaternion.Euler(new Vector3(0, index / (float)count * 360, 0)) * Vector3.right * range;
        }
    }
}