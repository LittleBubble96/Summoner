using UnityEngine;

namespace GameLogic.Game
{
    public interface ICharacterItemView
    {
        void SetVelocity(Vector3 v);
        void Damage();
        void Death();

        void DeathComplete();

        #region 动画

        void SetAnimationBool(string param, bool value);
        
        void SetAnimationFloat(string param, float value);

        #endregion
    }
}