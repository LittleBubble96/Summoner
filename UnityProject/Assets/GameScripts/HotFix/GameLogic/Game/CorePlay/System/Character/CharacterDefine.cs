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
}