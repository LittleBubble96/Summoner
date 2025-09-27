using GameFramework;
using GameFramework.Scene;
using GameLogic.Game.Common;
using UnityEngine.SceneManagement;

namespace GameLogic.Game
{
    public class UIHome : UIWindow
    {
        private XYButton _startButton;
        protected override void ScriptGenerator()
        {
            base.ScriptGenerator();
            _startButton = CreateWidget<XYButton>("Bg/m_playBtn");
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            if (_startButton != null)
            {
                _startButton.AddListener(OnStartButtonClick);
            }
        }

        private void OnStartButtonClick()
        {
            Close();
            GameMgr.Instance.StartGame();
        }
    }
}