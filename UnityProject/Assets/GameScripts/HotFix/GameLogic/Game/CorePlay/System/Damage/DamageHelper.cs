namespace GameLogic.Game
{
    public static class DamageHelper
    {
        public static int CalDamageValue(ActorInstanceId owner, DamageSourceType sourceType, DamageType damageType)
        {
            return CalDamageValue(CharacterManager.Instance.GetCharacter(owner), sourceType, damageType);
        }

        //计算伤害值
        public static int CalDamageValue(CharacterElement owner , DamageSourceType sourceType, DamageType damageType)
        {
            int value = 0;
            if (owner == null)
            {
                return value;
            }
            if (sourceType == DamageSourceType.MagicAttack)
            {
                value += owner.MagicAttack;
            }
            if (sourceType == DamageSourceType.PhysicalAttack)
            {
                value += owner.PhysicalAttack;
            }
            return damageType == DamageType.Damage ? value : -1 * value;
        }
    }
}