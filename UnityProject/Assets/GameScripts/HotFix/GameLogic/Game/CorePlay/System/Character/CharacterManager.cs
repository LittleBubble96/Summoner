using System.Collections.Generic;
using GameBase;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameLogic.Game
{
    public partial class CharacterManager : BaseLogicSys<CharacterManager>
    {

        public Dictionary<ActorInstanceId, CharacterElement> CharacterDic = new Dictionary<ActorInstanceId, CharacterElement>();
        private ICharacterView m_CharacterView;
        public ICharacterView CharacterView => m_CharacterView;
        public ActorInstanceId MainActorId { get; private set; }

        public override bool OnInit()
        {
            InitRelation();
            return base.OnInit();
        }

        public void InitCharacter()
        {
            CreateMainCharacter();
        }

        public void CreateCharacter(Vector3 position, CharacterFactionType factionType)
        {
            CharacterElement element = new CharacterElement();
            element.ActorInstanceId = ActorInstanceId.NewId();
            element.FactionType = factionType;
            element.Position = position;
            element.Init();
            CharacterDic.Add(element.ActorInstanceId,element);
            XYEvent.GEvent.Fire(this,EventDefine.CreateCharacterEventName, element);
        }

        private void CreateMainCharacter()
        {
            MainCharacter mainChar = ReferencePool.Acquire<MainCharacter>();//TODO 死亡需要销毁
            mainChar.FactionType = CharacterFactionType.Player;
            mainChar.ActorInstanceId = ActorInstanceId.NewId();
            MainActorId = mainChar.ActorInstanceId;
            mainChar.Init();
            CharacterView?.OnCreateMainCharacter(mainChar);
            CharacterDic.Add(MainActorId,mainChar);
        }
        
        /// <summary>
        /// 获取角色数据
        /// </summary>
        public CharacterElement GetCharacter(ActorInstanceId actorInstanceId)
        {
            if (CharacterDic.TryGetValue(actorInstanceId, out var character))
            {
                return character;
            }
            return null;
        }

        public void Inject(ICharacterView view)
        {
            m_CharacterView = view;
        }
        
        
    }
}