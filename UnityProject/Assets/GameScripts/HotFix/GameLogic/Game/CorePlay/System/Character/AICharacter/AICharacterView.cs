using System;
using UnityEngine;
using UnityEngine.AI;
using UnityGameFramework.Runtime;

namespace GameLogic.Game
{
    public class AICharacterView : CharacterBaseView
    {
        public AICharacter AICharacterData { get; set; }
        [SerializeField] private NavMeshAgent agent;

        protected override void OnInitCharacter()
        {
            base.OnInitCharacter();
            AICharacterData = CharacterElement as AICharacter;
        }

        public override void DoUpdate(float dt)
        {
            base.DoUpdate(dt);
            if (AICharacterData.IsNavToTarget)
            {
                agent.isStopped = false;
                agent.speed = AICharacterData.MoveSpeed;
                agent.SetDestination(AICharacterData.NavTargetPosition);
            }
            CharacterElement.SetPosition(transform.position);
        }
    }
}