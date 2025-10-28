using UnityEngine;

namespace GameLogic.Game
{
    public class CameraFixedPositionBehavior : CameraBehavior
    {
        private Vector3 _fixedPosition;
        private Quaternion _fixedRotation;

        public override void OnSetup(CommonArgs args)
        {
            if (args is CommonTwoArgs<Vector3,Quaternion> commonArgs)
            {
                _fixedPosition = commonArgs.Arg1;
                _fixedRotation = commonArgs.Arg2;
            }
        }

        public override void OnExecute(Camera camera, float dt)
        {
            var transform = camera.transform;
            transform.position = _fixedPosition;
            transform.rotation = _fixedRotation;
        }
        
        public override void Clear()
        {
            _fixedPosition = Vector3.zero;
            _fixedRotation = Quaternion.identity;
        }
    }
}