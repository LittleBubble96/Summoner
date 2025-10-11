using System;
using UnityGameFramework.Runtime;

namespace GameLogic.Game
{
    public class AICharacterView : CharacterBaseView
    {
        public AICharacter AICharacterData { get; set; }

        protected override void OnInitCharacter()
        {
            base.OnInitCharacter();
            AICharacterData = CharacterElement as AICharacter;
        }

        private void Update()
        {
            
        }
    }
}