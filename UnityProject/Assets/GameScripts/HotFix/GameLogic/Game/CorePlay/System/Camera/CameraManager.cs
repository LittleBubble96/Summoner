using System.Collections.Generic;
using GameBase;
using GameFramework;
using UnityEngine;

namespace GameLogic.Game
{
    public class CameraManager : BaseLogicSys<CameraManager>
    {
        private List<CameraProxy> _cameraProxies;
        public MainCameraProxy MainCameraProxy { get; set; }

        public override bool OnInit()
        {
            _cameraProxies = new List<CameraProxy>();
            return base.OnInit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            for (int i = _cameraProxies.Count - 1; i >= 0; i--)
            {
                _cameraProxies[i].OnUpdate(Time.deltaTime);
            }
        }

        public void StartScene()
        {
            MainCameraProxy = ReferencePool.Acquire<MainCameraProxy>();
            MainCameraProxy.Init(Camera.main);
            _cameraProxies.Add(MainCameraProxy);
        }

        public void ClearScene()
        {
            foreach (var camera in _cameraProxies)
            {
                ReferencePool.Release(camera);
            }
            _cameraProxies.Clear();
            
        }
    }
}