using System;
using System.Collections.Generic;

namespace GameLogic.Game
{
    public class BuffTypeConfig
    {
        public virtual Dictionary<EBuffType,Func<BuffItemBase>> BuffItemFactoryDic { get; } = new Dictionary<EBuffType, Func<BuffItemBase>>();
    }

    public enum EBuffType
    {
        None = 0,
        BaseDamage = 1, // 基础伤害
        Fire = 2,       // 火焰灼烧
        Freeze = 3,     // 冰冻减速
        Stun = 4,        // 击晕
    }
    
    /// <summary>
    /// Buff效果类型
    /// </summary>
    public enum BuffEffectType
    {
        InstantEffect,  // 即时效果
        OverTimeEffect, // 持续效果
    }
    
}