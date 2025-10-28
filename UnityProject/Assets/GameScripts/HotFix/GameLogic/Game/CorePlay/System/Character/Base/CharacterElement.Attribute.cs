using System;
using GameFramework;

namespace GameLogic.Game
{
    public partial class CharacterElement
    {
        public Action<CommonArgs> OnAttributeChanged { get; set; }

        #region 移动速度

        public float MoveSpeed => GetFloatAttribute_Internal(CharacterAttributeType.MoveSpeed);

        public void SetMoveSpeed(float value)
        {
            SetFloatAttribute_Internal(CharacterAttributeType.MoveSpeed,value);
        }
        
        #endregion

        #region 血量

        public int Hp =>  GetIntAttribute_Internal(CharacterAttributeType.HealthPoint);

        public void SetHp(int hp,DamageSourceType damageSourceType)
        {
            SetIntAttribute_Internal(CharacterAttributeType.HealthPoint ,hp);
            OnAttributeChanged?.Invoke(CommonArgs.CreateTwoArgs(CharacterAttributeType.HealthPoint,damageSourceType));
        }

        public void IncreaseHp(int value,DamageSourceType damageSourceType)
        {
            SetHp(Hp < value ? 0 : Hp - value,damageSourceType);
        }

        #endregion

        #region 物理攻击

        public int PhysicalAttack => GetIntAttribute_Internal(CharacterAttributeType.PhysicalBasicAttack); 
      
        public void SetPhysicalAttack(int value)
        {
            SetIntAttribute_Internal(CharacterAttributeType.PhysicalBasicAttack,value);
        }
        #endregion


        #region 攻击距离

        public int AttackDistance => GetIntAttribute_Internal(CharacterAttributeType.AttackDistance);

        public void SetAttackDistance(int value)
        {
            SetIntAttribute_Internal(CharacterAttributeType.AttackDistance,value);
        }

        #endregion

        #region 攻击速度

        public float AttackSpeed => GetFloatAttribute_Internal(CharacterAttributeType.AttackSpeed);
        public void SetAttackSpeed(float value)
        {
            SetFloatAttribute_Internal(CharacterAttributeType.AttackSpeed,value);
        }

        #endregion
        
        private int GetIntAttribute_Internal(CharacterAttributeType attributeType)
        {
            if (AttributeDic.TryGetValue(attributeType, out var attribute))
            {
                if (attribute is CharacterIntAttribute intAttribute)
                {
                    return intAttribute.Value;
                }
            }
            return 0;
        }

        private void SetIntAttribute_Internal(CharacterAttributeType attributeType,int value)
        {
            CharacterIntAttribute intAttribute = TryAddAttribute_Internal<CharacterIntAttribute>(attributeType);
            intAttribute.Value = value;
        }
        
        private float GetFloatAttribute_Internal(CharacterAttributeType attributeType)
        {
            if (AttributeDic.TryGetValue(attributeType, out var attribute))
            {
                if (attribute is CharacterFloatAttribute floatAttribute)
                {
                    return floatAttribute.Value;
                }
            }
            return 0;
        }

        private void SetFloatAttribute_Internal(CharacterAttributeType attributeType,float value)
        {
            CharacterFloatAttribute floatAttribute = TryAddAttribute_Internal<CharacterFloatAttribute>(attributeType);
            floatAttribute.Value = value;
        }
        
        private T TryAddAttribute_Internal<T>(CharacterAttributeType attributeType) where T : CharacterAttributeValue ,new()
        {
            if (AttributeDic.TryGetValue(attributeType , out var attributeValue))
            {
                if (attributeValue is T att)
                {
                    return att;
                }
                ReferencePool.Release(attributeValue);
                T attribute = ReferencePool.Acquire<T>();
                AttributeDic[attributeType] = attribute;
                return attribute;
            }
            else
            {
                T attribute = ReferencePool.Acquire<T>();
                AttributeDic[attributeType] = attribute;
                return attribute;
            }
        }
    }
}