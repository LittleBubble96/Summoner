using UnityEngine.UI;

namespace GameLogic.Game.Common
{
    public class XYButton : UIWidget
    {
        private Button _button;
        protected override void ScriptGenerator()
        {
            base.ScriptGenerator();
            _button = transform.GetComponent<Button>();
        }
        
        public void AddListener(UnityEngine.Events.UnityAction action)
        {
            if (_button != null)
            {
                _button.onClick.AddListener(action);
            }
        }
    }
}