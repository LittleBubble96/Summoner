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
        private Queue<ActorInstanceId> m_DestroyQueue = new Queue<ActorInstanceId>();
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

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var character in CharacterDic.Values)
            {
                character.DoUpdate(Time.deltaTime);
            }
            if (CharacterView!=null)
            {
                CharacterView.OnUpdate(Time.deltaTime);
            }
            ProcessDestroyQueue();
        }

        public void CreateAICharacter(int roleId , Vector3 position , Vector3 rotation, CharacterFactionType factionType)
        {
            AICharacter ai = new AICharacter();
            ai.ActorInstanceId = ActorInstanceId.NewId();
            ai.FactionType = factionType;
            ai.Position = position;
            ai.Rotation = rotation;
            ai.Init(CommonArgs.CreateOneArgs(roleId));
            CharacterView?.OnCreateAICharacter(ai);
            CharacterDic.Add(ai.ActorInstanceId,ai);
        }

        private void CreateMainCharacter()
        {
            MainCharacter mainChar = ReferencePool.Acquire<MainCharacter>();//TODO 死亡需要销毁
            mainChar.FactionType = CharacterFactionType.Player;
            mainChar.ActorInstanceId = ActorInstanceId.NewId();
            MainActorId = mainChar.ActorInstanceId;
            mainChar.Init(null);
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

        /// <summary>
        /// 销毁角色数据
        /// </summary>
        public void DestroyCharacter(ActorInstanceId actorInstanceId)
        {
            m_DestroyQueue.Enqueue(actorInstanceId);
            CharacterView?.OnDestroyCharacter(actorInstanceId);
        }
        
        private void ProcessDestroyQueue()
        {
            while (m_DestroyQueue.Count > 0)
            {
                var actorId = m_DestroyQueue.Dequeue();
                if (CharacterDic.TryGetValue(actorId,out var characterElement))
                {
                    CharacterDic.Remove(actorId);
                    ReferencePool.Release(characterElement);
                }
            }
        }

        public void Inject(ICharacterView view)
        {
            m_CharacterView = view;
        }
        
        
    }
}