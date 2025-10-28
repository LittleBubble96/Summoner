using GameBase;
using GameFramework;
using GameFramework.Resource;
using GameFramework.Scene;
using UnityEngine.SceneManagement;

namespace GameLogic.Game
{
    public enum EGameState
    {
        Init,
        Playing,
        End,
    }

    public class GameMgr : TSingleton<GameMgr>
    {
        private EGameState m_CurGameState = EGameState.Init;
        public EGameState CurGameState => m_CurGameState;
        private LoadSceneCallbacks LoadSceneCallbacks;
        public void StartGame()
        {
            m_CurGameState = EGameState.Init;
            LoadSceneCallbacks = new LoadSceneCallbacks(
                (sceneName, scene,dur,obj) =>
                {
                    StartGameReal();
                },
                (sceneName, status,msg, obj) => { },
                (sceneName, progress, obj) => { });
            GameFrameworkSystem.GetModule<ISceneManager>().LoadScene("game",LoadSceneCallbacks,LoadSceneMode.Single);

        }

        private void StartGameReal()
        {
            m_CurGameState = EGameState.Playing;
            UISystem.Instance.ShowUI<UICorePlay>();
            BuildingSystem.Instance.GenerateGrid();
            CameraManager.Instance.StartScene();
            CharacterManager.Instance.InitCharacter();
        }

        private void EndGame()
        {
            m_CurGameState = EGameState.End;
            UISystem.Instance.CloseUI<UICorePlay>();
            CharacterManager.Instance.ExitCharacter();
            CameraManager.Instance.ClearScene();
            GameFrameworkSystem.GetModule<ISceneManager>().UnloadScene("game");
        }
    }
}