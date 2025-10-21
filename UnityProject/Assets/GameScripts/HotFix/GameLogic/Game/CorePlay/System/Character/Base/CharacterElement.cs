using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace GameLogic.Game
{
    public partial class CharacterElement : IReference
    {
        public ActorInstanceId ActorInstanceId { get; set; }
        protected Vector3 Position;
        protected Vector3 Rotation;
        //手动控制朝向
        public bool IsManualControlRotation { get; set; }
        public Vector3 ManualControlRotation { get; set; }

        public CharacterFactionType FactionType = CharacterFactionType.Player;
        // 角色属性字典
        public Dictionary<CharacterAttributeType,CharacterAttributeValue> AttributeDic = new Dictionary<CharacterAttributeType, CharacterAttributeValue>();

        public void Init(CommonArgs args)
        {
            OnInit(args);
            OnInitAttribute();
            RegisterActorComponent();
        }
        
        protected virtual void OnInit(CommonArgs args)
        {
            
        }

        protected virtual void RegisterActorComponent()
        {
            RegisterComponent<TargetComponent>();
        }

        protected virtual void OnInitAttribute()
        {
            
        }

        public virtual void DoUpdate(float dt)
        {
            
        }

        public bool IsDead()
        {
            return Hp <= 0;
        }

        public void SetPosition(Vector3 targetPos)
        {
            Position = targetPos;
        }
        
        public Vector3 GetPosition()
        {
            return Position;
        }
        
        public void SetRotation(Vector3 targetRot)
        {
            Rotation = targetRot;
        }
        
        public Vector3 GetRotation()
        {
            return Rotation;
        }
        
        #region 手动控制旋转

        public void ManualControlRotationStart(Vector3 eulerAngles)
        {
            IsManualControlRotation = true;
            ManualControlRotation = eulerAngles;
        }
        
        public void CancelManualControlRotation()
        {
            IsManualControlRotation = false;
            ManualControlRotation = Vector3.zero;
        }

        #endregion

        #region 动画

        public void SetAnimationBool(string animName , bool value)
        {
            XYEvent.GEvent.FireNow(this,EventDefine.CharacterAnimationSetBoolEventName,this.ActorInstanceId,animName,value);
        }
        

        #endregion


        public virtual void Clear()
        {
            Position = Vector3.zero;
            Rotation = Vector3.zero;
            FactionType = CharacterFactionType.Player;
            AttributeDic.Clear();
            OnAttributeChanged = null;
            ClearComponent();
        }
    }

    /// <summary>
    /// 角色属性
    /// </summary>
    public class CharacterAttributeValue : IReference
    {
        public virtual void Clear()
        {
            
        }
    }
    
    public class CharacterFloatAttribute : CharacterAttributeValue
    {
        public float Value { get; set; }

        public override void Clear()
        {
            base.Clear();
            Value = 0;
        }
    }
    
    public class CharacterIntAttribute : CharacterAttributeValue
    {
        public int Value { get; set; }

        public override void Clear()
        {
            base.Clear();
            Value = 0;
        }
    }
}