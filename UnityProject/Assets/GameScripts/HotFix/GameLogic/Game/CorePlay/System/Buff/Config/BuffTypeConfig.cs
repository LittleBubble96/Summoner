using System;
using System.Collections.Generic;

namespace GameLogic.Game
{
    public class BuffTypeConfig
    {
        public virtual Dictionary<EBuffType,Func<IBuffItem>> BuffItemFactoryDic { get; } = new Dictionary<EBuffType, Func<IBuffItem>>();
        public virtual Dictionary<EBuffType,Func<IBuffItemView>> BuffItemViewFactoryDic { get; } = new Dictionary<EBuffType, Func<IBuffItemView>>();
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