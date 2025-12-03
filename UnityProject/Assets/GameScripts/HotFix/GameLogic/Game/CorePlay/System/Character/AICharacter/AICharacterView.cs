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
        private float _damageTimerCount = 0;
    
        protected override void OnInitCharacter()
        {
            AICharacterData = CharacterElement as AICharacter;
            agent = GetComponent<NavMeshAgent>();
            m_animator = GetComponentInChildren<Animator>();
            if (AICharacterData != null)
            {
                AICharacterData.SetAnimationBool("Death", false);
                AICharacterData.SetAnimationBool("Attack", false);
                AICharacterData.SetAnimationBool("Damage", false);
            }
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
            UpdateDamage(dt);
        }

        private void UpdateDamage(float dt)
        {
            if (_damageTimerCount > 0)
            {
                _damageTimerCount -= dt;
                if (_damageTimerCount <=0)
                {
                    StopDamage();
                }
            }
        }

        private void StopDamage()
        {
            _damageTimerCount = 0;
            AICharacterData.SetAnimationBool("Damage",false);
        }

        public override void Death()
        {
            base.Death();
            StopDamage();
            AICharacterData.SetAnimationBool("Death",true);
        }

        public override void SetVelocity(Vector3 v)
        {
            base.SetVelocity(v);
            agent.velocity = v;
        }

        public override void Damage()
        {
            base.Damage();
            if (AICharacterData.IsDead())
            {
                return;
            }
            AICharacterData.SetAnimationBool("Damage",true);
            _damageTimerCount = 0.375f;
            agent.velocity = Vector3.zero;
        }
    }
}