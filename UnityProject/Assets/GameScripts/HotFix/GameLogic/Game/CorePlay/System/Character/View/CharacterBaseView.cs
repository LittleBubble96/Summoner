using UnityEngine;

namespace GameLogic.Game
{
    public class CharacterBaseView : MonoBehaviour
    {
        //阵营
        public CharacterElement CharacterElement { get; private set; }
        
        public void Init(CharacterElement character)
        {
            CharacterElement = character;
            CharacterElement.OnAttributeChanged += OnAttributeChanged;
        }
        
        protected virtual void OnInitCharacter()
        {
            
        }

        protected virtual void OnAttributeChanged(CommonArgs args)
        {
            
        }
    }
}