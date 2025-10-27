using System;
using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;

namespace GameLogic.Game
{
    public partial class CharacterElement
    {
        private Dictionary<Type, ActorComponent> m_ActorComponents = new Dictionary<Type, ActorComponent>();
        
        private void RegisterComponent<T>() where T : ActorComponent , new()
        {
            if (m_ActorComponents.ContainsKey(typeof(T)))
            {
                Log.Warning($"角色已经存在该组件{typeof(T)}");
                return;
            }    
            m_ActorComponents.Add(typeof(T), ReferencePool.Acquire<T>());
        }

        public T GetComponent<T>() where T : ActorComponent, new()
        {
            if (m_ActorComponents.TryGetValue(typeof(T),out var component))
            {
                return component as T;
            }
            return null;
        }

        private void ClearComponent()
        {
            foreach (var component in m_ActorComponents)
            {
                ReferencePool.Release(component.Value);
            }
            m_ActorComponents.Clear();
        }
    }
}