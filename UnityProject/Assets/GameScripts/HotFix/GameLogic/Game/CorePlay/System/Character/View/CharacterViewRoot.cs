using System;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameLogic.Game
{
    public class CharacterViewRoot : MonoBehaviour ,ICharacterView
    {
        public Dictionary<ActorInstanceId,CharacterBaseView> CharacterViewDic = new Dictionary<ActorInstanceId, CharacterBaseView>();
        private Queue<ActorInstanceId> _destroyQueue = new Queue<ActorInstanceId>();
        private void Awake()
        {
            CharacterManager.Instance.Inject(this);
        }

        #region 接口

        public void OnUpdate(float dt)
        {
            foreach (var character in CharacterViewDic)
            {
                character.Value.DoUpdate(dt);
            }
            //处理角色销毁逻辑
            ProcessDestroyCharacter();
        }

        public void OnDestroyCharacter(ActorInstanceId actorInstanceId)
        {
            _destroyQueue.Enqueue(actorInstanceId);
        }

        public void OnCreateMainCharacter(MainCharacter character)
        {
            GameObject role = PoolManager.Instance.GetGameObject(CharacterDefine.MainCharacterAsset,transform);
            MainCharacterView view = role.GetOrAddComponent<MainCharacterView>();
            view.Init(character);
            CharacterViewDic.Add(character.ActorInstanceId,view);
        }

        public void OnCreateAICharacter(AICharacter character)
        {
            GameObject ai = PoolManager.Instance.GetGameObject(character.RoleConfig.ResPath,transform);
            ai.transform.position = character.GetPosition();
            ai.transform.rotation = Quaternion.Euler(character.GetRotation());
            AICharacterView view = ai.GetOrAddComponent<AICharacterView>();
            view.Init(character);
            CharacterViewDic.Add(character.ActorInstanceId,view);
        }

        #endregion

        private void ProcessDestroyCharacter()
        {
            while (_destroyQueue.Count > 0)
            {
                ActorInstanceId actorInstanceId = _destroyQueue.Dequeue();
                if (CharacterViewDic.TryGetValue(actorInstanceId,out CharacterBaseView view))
                {
                    CharacterViewDic.Remove(actorInstanceId);
                    PoolManager.Instance.PushObject(view);
                }
            }
        }
    }
}