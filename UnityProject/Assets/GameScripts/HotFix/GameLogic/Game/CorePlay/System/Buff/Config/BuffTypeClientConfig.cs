using System;
using System.Collections.Generic;
using GameFramework;

namespace GameLogic.Game
{
    public class BuffTypeClientConfig : BuffTypeConfig
    {
        public override Dictionary<EBuffType, Func<IBuffItem>> BuffItemFactoryDic =>
            new Dictionary<EBuffType, Func<IBuffItem>>()
            {
                { EBuffType.BaseDamage, ReferencePool.Acquire<BaseDamageBuff> },
                { EBuffType.Fire, ReferencePool.Acquire<FireBuffItem> },
                { EBuffType.Freeze, ReferencePool.Acquire<FreezeBuffItem> },
                { EBuffType.Stun, ReferencePool.Acquire<StunBuffItem> }
            };
    }
}