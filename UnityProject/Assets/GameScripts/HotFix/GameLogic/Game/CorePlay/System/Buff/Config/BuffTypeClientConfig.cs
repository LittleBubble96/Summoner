using System;
using System.Collections.Generic;
using GameFramework;

namespace GameLogic.Game
{
    public class BuffTypeClientConfig : BuffTypeConfig
    {
        public override Dictionary<EBuffType, Func<BuffItemBase>> BuffItemFactoryDic =>
            new Dictionary<EBuffType, Func<BuffItemBase>>()
            {
                { EBuffType.BaseDamage, ReferencePool.Acquire<BaseDamageBuffView> },
                { EBuffType.Fire, ReferencePool.Acquire<FireBuffViewItem> },
                { EBuffType.Freeze, ReferencePool.Acquire<FreezeBuffViewItem> },
                { EBuffType.Stun, ReferencePool.Acquire<StunBuffViewItem> }
            };
    }
}