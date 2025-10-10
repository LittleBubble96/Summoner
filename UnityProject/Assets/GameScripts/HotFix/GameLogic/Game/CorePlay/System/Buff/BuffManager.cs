using System.Collections.Generic;
using GameBase;
using UnityEngine;

namespace GameLogic.Game
{
    public class BuffManager : BaseLogicSys<BuffManager>
    {
        private BuffTypeConfig _config;
        private Dictionary<BuffInstanceId, BuffItemBase> _buffItems = new Dictionary<BuffInstanceId, BuffItemBase>();

        private Dictionary<ActorInstanceId, List<BuffInstanceId>> _actorAttachBuffs = new Dictionary<ActorInstanceId, List<BuffInstanceId>>();
         
        public void InitConfig(BuffTypeConfig config)
        {
            _config = config;
        }

        public void AddBuff(EBuffType buffType, ActorInstanceId actorId)
        {
            if (!_actorAttachBuffs.TryGetValue(actorId,out var actorAttachBuff))
            {
                actorAttachBuff = new List<BuffInstanceId>();
                _actorAttachBuffs.Add(actorId,actorAttachBuff);
            }

            int existingBuffIndex = actorAttachBuff.FindIndex((id) =>
            {
                if (_buffItems.TryGetValue(id,out var buff))
                {
                    return buff.GetBuffType() == buffType;
                }
                return false;
            });
            if (existingBuffIndex >= 0)
            {
                BuffItemBase buffItem = _buffItems[actorAttachBuff[existingBuffIndex]];
                buffItem.RefreshBuff();
            }
            else
            {
                BuffItemBase buffItem = CreateBuffItem(buffType);
                BuffInstanceId buffId = BuffInstanceId.NewId();
                _buffItems.Add(buffId,buffItem);
                actorAttachBuff.Add(buffId);
                buffItem.InitBuff(null);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var actorAttachBuff in _actorAttachBuffs)
            {
                List<BuffInstanceId> actorAttachBuffIdList = actorAttachBuff.Value;
                for (int i = actorAttachBuffIdList.Count - 1; i >= 0; i--)
                {
                    if (_buffItems.TryGetValue(actorAttachBuffIdList[i],out var buff))
                    {
                        if (buff.ExecuteTime >= buff.GetInternalTime())
                        {
                            bool canExecute = buff.ExecuteCount < buff.GetExecuteCount();
                            if (canExecute)
                            {
                                buff.ExecuteBuff();
                            }
                            else
                            {
                                RemoveBuff(actorAttachBuff.Key , actorAttachBuffIdList[i]);
                            }
                        }
                        buff.ExecuteTime += Time.deltaTime;
                    }
                }
            }
        }

        private void RemoveBuff(ActorInstanceId actorId , BuffInstanceId buffId)
        {
            _buffItems.Remove(buffId, out var buff);
            if (buff != null)
            {
                buff.RemoveBuff();
            }

            if (_actorAttachBuffs.TryGetValue(actorId, out var actorAttachBuffs))
            {
                actorAttachBuffs.Remove(buffId);
            }
        }

        /// <summary>
        /// 创建逻辑buff
        /// </summary>
        private BuffItemBase CreateBuffItem(EBuffType buffType)
        {
            if (_config == null)
            {
                return null;
            }
            if (_config.BuffItemFactoryDic.TryGetValue(buffType, out var itemFactory))
            {
                return itemFactory();
            }
            return null;
        }

        public void ClearBuffs()
        {
            _buffItems.Clear();
        }
    }
}