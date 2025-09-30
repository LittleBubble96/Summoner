using GameFramework;

namespace GameLogic.Game
{
    public partial class CharacterElement
    {
        #region 移动速度

        public float GetMoveSpeed()
        {
            return GetFloatAttribute_Internal(CharacterAttributeType.MoveSpeed);
        }
        
        public void SetMoveSpeed(float value)
        {
            SetFloatAttribute_Internal(CharacterAttributeType.MoveSpeed,value);
        }
        
        #endregion

        #region 血量

        public float GetHp()
        {
            return GetIntAttribute_Internal(CharacterAttributeType.HealthPoint);
        }

        public void SetHp(int hp)
        {
            SetIntAttribute_Internal(CharacterAttributeType.HealthPoint ,hp);
        }

        #endregion
        
        private float GetIntAttribute_Internal(CharacterAttributeType attributeType)
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