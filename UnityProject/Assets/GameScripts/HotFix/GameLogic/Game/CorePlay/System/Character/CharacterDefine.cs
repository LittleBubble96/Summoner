namespace GameLogic.Game
{
    public class CharacterDefine
    {
        public const string MainCharacterAsset = "role1";
    }

    /// <summary>
    /// 角色阵营
    /// </summary>
    public enum CharacterFactionType
    {
        None = 0,
        Player = 1 << 0,
        Enemy = 1 << 1,
        Neutral = 1 << 2,
        All = Player | Enemy | Neutral
    }

    public enum FactionRelationType
    {
        Hostile, //敌对 可攻击
        Friendly, //友好不可 攻击
        Neutral , // 中立
    }

    public enum DamageSourceType
    {
        None = 0,
        PhysicalAttack = 1 << 0, //物理攻击
        MagicAttack = 1 << 1, //魔法攻击
        TrueAttack = 1 << 2, //真实伤害
        Heal = 1 << 3, //治疗
    }

    public enum CharacterAttributeType
    {
        //物理基础攻击
        PhysicalBasicAttack,
        //魔法基础攻击
        MagicBasicAttack,
        //物理防御
        PhysicalDefense,
        //魔法防御
        MagicDefense,
        //生命值
        HealthPoint,
        //生命值上限
        HealthPointMax,
        //法力值
        ManaPoint,
        //法力值上限
        ManaPointMax,
        //移动速度
        MoveSpeed,
        //暴击率
        CriticalRate,
        //暴击伤害
        CriticalDamage,
        //生命偷取
        LifeSteal,
        //攻击速度
        AttackSpeed,
        //攻击距离
        AttackDistance,
    }
}