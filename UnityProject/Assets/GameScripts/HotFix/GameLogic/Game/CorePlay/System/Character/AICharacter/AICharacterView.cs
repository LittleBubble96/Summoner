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

        public override void DoUpdate(float dt)
        {
            base.DoUpdate(dt);
            CharacterElement.SetPosition(transform.position);
        }
    }
}