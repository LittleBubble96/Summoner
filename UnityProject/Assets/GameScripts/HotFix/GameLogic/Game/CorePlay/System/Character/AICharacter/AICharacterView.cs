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
                agent.updateRotation = false;
                if (!agent.pathPending) //路径是否准备完毕
                {
                    AICharacterData.NavToTargetRemainDistance = agent.remainingDistance;
                    // Log.Info($"[AI] 相对距离：{AICharacterData.NavToTargetRemainDistance}");
                }

                if (agent.velocity.magnitude > 0.1f )
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(agent.velocity), 10 * dt);
                }
                AICharacterData.SetAnimationBool("Move",true);
            }
            else
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                AICharacterData.SetAnimationBool("Move",false);
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