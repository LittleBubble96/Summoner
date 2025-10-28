using System;
using UnityEngine;
using UnityEngine.AI;
using UnityGameFramework.Runtime;

namespace GameLogic.Game
{
    public class AICharacterView : CharacterBaseView
    {
        public AICharacter AICharacterData { get; set; }
        private NavMeshAgent agent;
    
        protected override void OnInitCharacter()
        {
            AICharacterData = CharacterElement as AICharacter;
            agent = GetComponent<NavMeshAgent>();
            m_animator = GetComponentInChildren<Animator>();
        }

        protected override void DoUpdate_Internal(float dt)
        {
            if (AICharacterData.IsNavToTarget)
            {
                agent.isStopped = false;
                agent.speed = AICharacterData.MoveSpeed;
                agent.SetDestination(AICharacterData.NavTargetPosition);
                agent.updateRotation = AICharacterData.NavUpdateRotation;
                AICharacterData.NavToTargetRemainDistance = agent.remainingDistance;
            }
            else
            {
                agent.isStopped = true;
            }

            if (AICharacterData.IsManualControlRotation)
            {
                transform.eulerAngles = AICharacterData.ManualControlRotation;
            }
            CharacterElement.SetPosition(transform.position);
            CharacterElement.SetRotation(transform.eulerAngles);
        }
    }
}