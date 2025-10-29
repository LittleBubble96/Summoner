namespace GameLogic.Game
{
    public interface ICharacterItemView
    {
        void Death();

        void DeathComplete();

        #region 动画

        void SetAnimationBool(string param, bool value);
        
        void SetAnimationFloat(string param, float value);

        #endregion
    }
}