using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace GameLogic.Game
{
    public partial class CharacterElement : IReference
    {
        public ActorInstanceId ActorInstanceId { get; set; }
        public Vector3 Position;
        public CharacterFactionType FactionType = CharacterFactionType.Player;
        // 角色属性字典
        public Dictionary<CharacterAttributeType,CharacterAttributeValue> AttributeDic = new Dictionary<CharacterAttributeType, CharacterAttributeValue>();

        public void Init()
        {
            OnInit();
        }
        
        protected virtual void OnInit()
        {
            
        }

        public bool IsDead()
        {
            return Hp <= 0;
        }

        public void Clear()
        {
            Position = Vector3.zero;
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