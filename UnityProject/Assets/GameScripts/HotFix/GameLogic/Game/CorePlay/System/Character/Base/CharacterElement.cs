using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace GameLogic.Game
{
    public partial class CharacterElement : IReference
    {
        public ActorInstanceId ActorInstanceId { get; set; }
        public Vector3 Position;
        public Vector3 Rotation;
        public CharacterFactionType FactionType = CharacterFactionType.Player;
        // 角色属性字典
        public Dictionary<CharacterAttributeType,CharacterAttributeValue> AttributeDic = new Dictionary<CharacterAttributeType, CharacterAttributeValue>();

        public void Init(CommonArgs args)
        {
            OnInit(args);
            OnInitAttribute();
        }
        
        protected virtual void OnInit(CommonArgs args)
        {
            
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

        public virtual void Clear()
        {
            Position = Vector3.zero;
            Rotation = Vector3.zero;
            FactionType = CharacterFactionType.Player;
            AttributeDic.Clear();
            OnAttributeChanged = null;
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