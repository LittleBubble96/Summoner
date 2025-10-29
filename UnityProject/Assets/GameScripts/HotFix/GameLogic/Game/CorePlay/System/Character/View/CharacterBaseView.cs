using System;
using GameFramework.Event;
using UnityEngine;

namespace GameLogic.Game
{
    public class CharacterBaseView : MonoBehaviour,ICharacterItemView
    {
        protected Animator m_animator;
        //阵营
        public CharacterElement CharacterElement { get; private set; }
        
        public void Init(CharacterElement character)
        {
            CharacterElement = character;
            CharacterElement.OnAttributeChanged += OnAttributeChanged;
            OnInitCharacter();
        }

        protected virtual void OnInitCharacter()
        {
            
        }

        public void DoUpdate(float dt)
        {
            DoUpdate_Internal(dt);
        }
        
        protected virtual void DoUpdate_Internal(float dt)
        {
            
        }

        // 属性变化回调
        protected virtual void OnAttributeChanged(CommonArgs args)
        {
            
        }
        
        //死亡回调
        public virtual void Death()
        {
            
        }
        
        //死亡完成回调
        public virtual void DeathComplete()
        {
            
        }

        #region 动画

        public virtual void SetAnimationBool(string param, bool value)
        {
            if (m_animator)
            {
                m_animator.SetBool(param,value);
            }
        }
        
        public virtual void SetAnimationFloat(string param, float value)
        {
            if (m_animator)
            {
                m_animator.SetFloat(param,value);
            }
        }

        #endregion
    }
}