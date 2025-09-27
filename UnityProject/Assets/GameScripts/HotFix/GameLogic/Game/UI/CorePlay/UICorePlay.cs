using GameFramework.Event;
using UnityEngine;

namespace GameLogic.Game
{
    public class UICorePlay : UIWindow
    {
        private RectTransform m_downInput;
        private RectTransform m_dirInput;
        private float m_maxRadius = 200f;
        protected override void ScriptGenerator()
        {
            base.ScriptGenerator();
            m_downInput = FindChildComponent<RectTransform>("Bg/InputView/m_downInput");
            m_dirInput = FindChildComponent<RectTransform>("Bg/InputView/m_dirInput");
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