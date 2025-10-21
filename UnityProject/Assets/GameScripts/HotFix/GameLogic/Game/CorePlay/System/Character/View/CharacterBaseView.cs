using System;
using GameFramework.Event;
using UnityEngine;

namespace GameLogic.Game
{
    public class CharacterBaseView : MonoBehaviour
    {
        [SerializeField] protected Animator m_animator;
        //阵营
        public CharacterElement CharacterElement { get; private set; }
        
        public void Init(CharacterElement character)
        {
            CharacterElement = character;
            CharacterElement.OnAttributeChanged += OnAttributeChanged;
        }

        private void OnEnable()
        {
            XYEvent.GEvent.Subscribe(EventDefine.CharacterAnimationSetBoolEventName,OnAnimationSetBool);
        }

        private void OnDisable()
        {
            XYEvent.GEvent.Unsubscribe(EventDefine.CharacterAnimationSetBoolEventName,OnAnimationSetBool);
        }

        protected virtual void OnInitCharacter()
        {
            
        }

        public void DoUpdate(float dt)
        {
            DoUpdate_Internal(dt);
        }

        private void OnAnimationSetBool(object sender, GameEventArgs eventArgs)
        {
            if (!m_animator)
            {
                return;
            }
            if (eventArgs is GameEventCustomThreeParam<ActorInstanceId,string,bool> args)
            {
                if (CharacterElement.ActorInstanceId != args.Param)
                {
                    return;
                }
                m_animator.SetBool(args.Param1,args.Param2);
            }
        }
        

        protected virtual void DoUpdate_Internal(float dt)
        {
            
        }

        protected virtual void OnAttributeChanged(CommonArgs args)
        {
            
        }
    }
}