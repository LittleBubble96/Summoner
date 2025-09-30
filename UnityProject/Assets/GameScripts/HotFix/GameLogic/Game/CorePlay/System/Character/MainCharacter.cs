using UnityEngine;

namespace GameLogic.Game
{
    public class MainCharacter : CharacterElement
    {
        public float AngleSpeed = 720f; // degrees per second
        protected override void OnInit()
        {
            base.OnInit();
            SetMoveSpeed(4f);
        }
    }
}