using System;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameLogic.Game
{
    public class CharacterViewRoot : MonoBehaviour ,ICharacterView
    {
        private void Awake()
        {
            CharacterManager.Instance.Inject(this);
        }
        
        private void OnEnable()
        {
            XYEvent.GEvent.Subscribe(EventDefine.CreateCharacterEventName,OnCreateCharacter);
        }

        private void OnDisable()
        {
            XYEvent.GEvent.Unsubscribe(EventDefine.CreateCharacterEventName,OnCreateCharacter);
        }
        
        public void OnCreateCharacter(object sender,GameEventArgs e)
        {
            if (e == null)
            {
                Log.Error("CharacterViewRoot OnCreateCharacter error");
                return;
            }

            if (e is GameEventCustomOneParam<CharacterElement> eventArgs)
            {
                GameModule.Resource.LoadGameObject("Monster_Batty_A");
            }
        }

        #region 接口

        public void OnCreateMainCharacter(MainCharacter character)
        {
            GameObject role = GameModule.Resource.LoadGameObject(CharacterDefine.MainCharacterAsset,transform);
            MainCharacterView view = role.GetOrAddComponent<MainCharacterView>();
            view.Init(character);
        }

        #endregion
    }
}