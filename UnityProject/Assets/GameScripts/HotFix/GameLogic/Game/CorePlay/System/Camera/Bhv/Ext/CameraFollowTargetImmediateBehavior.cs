using UnityEngine;

namespace GameLogic.Game
{
    public class CameraFollowTargetImmediateBehavior : CameraBehavior
    {
        private Transform _targetTransform;
        private Vector3 _offset;
        private Quaternion _rotationOffset;
        public override void OnSetup(CommonArgs args)
        {
            if (args is CommonThreeArgs<Transform,Vector3,Quaternion> commonArgs)
            {
                _targetTransform = commonArgs.Arg1;
                _offset = commonArgs.Arg2;
                _rotationOffset = commonArgs.Arg3;
            }
        }
        
        public override void OnExecute(Camera camera, float dt)
        {
            if (_targetTransform == null)
            {
                return;
            }
            var transform = camera.transform;
            Vector3 desiredPosition = _targetTransform.position + _offset;
            transform.position = desiredPosition;
            Quaternion desiredRotation = _rotationOffset;
            transform.rotation = desiredRotation;
        }
        
        public override void Clear()
        {
            _targetTransform = null;
            _offset = Vector3.zero;
            _rotationOffset = Quaternion.identity;
        }
    }
}