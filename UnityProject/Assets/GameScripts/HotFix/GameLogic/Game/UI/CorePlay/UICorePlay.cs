using GameFramework.Event;
using GameLogic.Game.Common;
using UnityEngine;

namespace GameLogic.Game
{
    public class UICorePlay : UIWindow
    {
        private RectTransform m_downInput;
        private RectTransform m_dirInput;
        private float m_maxRadius = 200f;
        private XYButton m_button;
        private XYButton m_button1003;
        private XYButton m_button1004;

        protected override void ScriptGenerator()
        {
            base.ScriptGenerator();
            m_downInput = FindChildComponent<RectTransform>("Bg/InputView/m_downInput");
            m_dirInput = FindChildComponent<RectTransform>("Bg/InputView/m_dirInput");
            m_button = CreateWidget<XYButton>("Bg/test_button/m_createBtn");
            m_button1003 = CreateWidget<XYButton>("Bg/test_button/m_createBtn1003");
            m_button1004 = CreateWidget<XYButton>("Bg/test_button/m_createBtn1004");

        }
        
        protected override void OnRefresh()
        {
            base.OnRefresh();
            m_downInput.gameObject.SetActive(false);
            m_dirInput.gameObject.SetActive(false);
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            XYEvent.GEvent.Subscribe(EventDefine.PlayerControllerDownEventName, OnPlayerControllerDown);
            XYEvent.GEvent.Subscribe(EventDefine.PlayerControllerUpEventName, OnPlayerControllerUp);
            XYEvent.GEvent.Subscribe(EventDefine.PlayerControllerDragEventName, OnPlayerControllerDrag);
            m_button.AddListener(OnClickCreateBtn);
            m_button1003.AddListener(OnClickCreateBtn1003);
            m_button1004.AddListener(OnClickCreateBtn1004);

        }

        private void OnClickCreateBtn()
        {
            CharacterManager.Instance.CreateAICharacter(1001, Vector3.zero, Vector3.zero, CharacterFactionType.Player);
        }
        
        private void OnClickCreateBtn1003()
        {
            CharacterManager.Instance.CreateAICharacter(1003, Vector3.zero, Vector3.zero, CharacterFactionType.Player);
        }
        
        private void OnClickCreateBtn1004()
        {
            CharacterManager.Instance.CreateAICharacter(1004, Vector3.zero, Vector3.zero, CharacterFactionType.Player);
        }

        private void OnPlayerControllerDrag(object sender, GameEventArgs e)
        {
            if (e is GameEventCustomOneParam<Vector3> eventParam)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)UISystem.Instance.UICanvas.transform, eventParam.Param, UISystem.Instance.UICamera, out var uiPos);
                Vector2 dir = uiPos - m_downInput.anchoredPosition;
                if (dir.magnitude > m_maxRadius)
                {
                    dir = dir.normalized * m_maxRadius;
                }
                m_dirInput.anchoredPosition = m_downInput.anchoredPosition + dir;
            }
        }

        private void OnPlayerControllerUp(object sender, GameEventArgs e)
        {
            m_downInput.gameObject.SetActive(false);
            m_dirInput.gameObject.SetActive(false);
        }

        private void OnPlayerControllerDown(object sender, GameEventArgs e)
        {
            m_downInput.gameObject.SetActive(true);
            m_dirInput.gameObject.SetActive(true);
            if (e is GameEventCustomOneParam<Vector3> eventParam)
            {
                //鼠标输入转换为UI坐标
                RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)UISystem.Instance.UICanvas.transform, eventParam.Param, UISystem.Instance.UICamera, out var uiPos);
                m_downInput.anchoredPosition = uiPos;
            }
        }
    }
}