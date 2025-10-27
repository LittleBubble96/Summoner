using UnityEngine;

namespace GameLogic.Game
{
    public class MainCharacter : CharacterElement
    {
        public float AngleSpeed = 720f; // degrees per second
        protected override void OnInit(CommonArgs args)
        {
            base.OnInit(args);
            SetMoveSpeed(4f);
            SetHp(1,DamageSourceType.None);
        }
    }
}