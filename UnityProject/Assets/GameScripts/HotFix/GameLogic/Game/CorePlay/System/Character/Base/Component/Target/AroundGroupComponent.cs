using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameLogic.Game
{
    public class AroundGroupComponent : ActorComponent
    {
        private float _range;

        private float _rotateSpeed;

        private float _curRotate;
        
        private Dictionary<ActorInstanceId,AroundElement> _aroundElements;

        public ActorInstanceId AttachActorId { get; set; }


        public void Init(float range , float rotateSpeed)
        {
            _range = range;
            _rotateSpeed = rotateSpeed;
            _aroundElements = new Dictionary<ActorInstanceId, AroundElement>();
            _curRotate = 0;
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

        public void RemoveAround(ActorInstanceId actorInstanceId)
        {
            if (_aroundElements.ContainsKey(actorInstanceId))
            {
                return;
            }
            _aroundElements.Remove(actorInstanceId);
        }

        public Vector3? GetTarget(ActorInstanceId actorInstanceId)
        {
            if (_aroundElements.TryGetValue(actorInstanceId,out var aroundElement))
            {
                return  Quaternion.Euler(new Vector3(0, _curRotate, 0)) * aroundElement.GetTarget();
            }

            return null;
        }
        

        public override void DoUpdate(float dt)
        {
            UpdateSelf(dt);
            UpdateAttachPos();
        }

        private void UpdateSelf(float dt)
        {
            _curRotate += _rotateSpeed * dt;
            _curRotate %= 360;
        }

        //更新位置信息
        private void UpdateAttachPos()
        {
            if (AttachActorId.IsValid())
            {
                CharacterElement attached = CharacterManager.Instance.GetCharacter(AttachActorId);
                Vector3? target = attached.GetComponent<AroundGroupComponent>().GetTarget(OwnerId);
                CharacterElement self = CharacterManager.Instance.GetCharacter(OwnerId);
                if (target != null && !self.IsDead() && self is AICharacter aiCharacter)
                {
                    aiCharacter.NavToTarget(target.Value + attached.GetPosition());
                }
            }
        }


        public override void Clear()
        {
            base.Clear();
            _range = 0;
            foreach (var aroundElement in _aroundElements)
            {
                ReferencePool.Release(aroundElement.Value);
            }
            _aroundElements.Clear();
            _curRotate = 0;
            _rotateSpeed = 0;
            AttachActorId = default;
        }
    }

    public class AroundElement : IReference
    {
        private Vector3 _targetPos;
        
        
        public void SetTarget(int index , int count , float range)
        {
            _targetPos = CalTargetPos(index, count, range);
          
        }

        public void DoUpdate(float dt)
        {
            
        }

        public Vector3 GetTarget()
        {
            return _targetPos;
        }

        public void Clear()
        {
            _targetPos = Vector3.zero;
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