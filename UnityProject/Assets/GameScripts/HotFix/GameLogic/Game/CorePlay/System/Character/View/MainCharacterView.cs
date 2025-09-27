using System;
using System.Drawing.Printing;
using UnityEngine;

namespace GameLogic.Game
{
    public class MainCharacterView : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;
        [SerializeField] private CharacterController m_characterController;
        
        public MainCharacter Character { get; set; }
        
        private Vector3 m_inputDownPos;
        private Vector3 m_inputCurrentPos;
        private static readonly int IsRunningAnimHash = Animator.StringToHash("IsRunning");
        private bool IsRunning = false;
        
        public void Init(MainCharacter character)
        {
            Character = character;
        }

        private void Update()
        {
            if (Character == null)
            {
                return;
            }
            float speed = Character.Speed;
            float angleSpeed = Character.AngleSpeed;
            
            // 获取输入
            if (Input.GetMouseButtonDown(0))
            {
                m_inputDownPos = Input.mousePosition;
                XYEvent.GEvent.Fire(this, EventDefine.PlayerControllerDownEventName, Input.mousePosition);
                IsRunning = true;
            }
            if (Input.GetMouseButton(0))
            {
                m_inputCurrentPos = Input.mousePosition;
                Vector3 direction = m_inputCurrentPos - m_inputDownPos;
                direction.z = direction.y;
                direction.y = 0;
                if (direction.magnitude > 10f)
                {
                    direction.Normalize();
                    // 计算目标角度
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    // 移动角色
                    Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                    // 平滑旋转到目标角度
                    float angle = GetAngle(transform.forward, moveDir);
                    float maxAngleChange = angleSpeed * Time.deltaTime;
                    float angleChange = Mathf.Clamp(angle, -maxAngleChange, maxAngleChange);
                    transform.Rotate(0, angleChange, 0);
                    
                    m_characterController.Move(moveDir * speed * Time.deltaTime);
                    // 更新位置
                    Character.Position = transform.position;
                    // 播放行走动画
                    m_animator.SetBool(IsRunningAnimHash, true);
                }
                else
                {
                    // 停止移动，播放待机动画
                    m_animator.SetBool(IsRunningAnimHash, false);
                }
                XYEvent.GEvent.Fire(this, EventDefine.PlayerControllerDragEventName, Input.mousePosition);
            }
            else
            {
                // 停止移动，播放待机动画
                m_animator.SetBool(IsRunningAnimHash, false);
                if (IsRunning)
                {
                    IsRunning = false;
                    XYEvent.GEvent.Fire(this, EventDefine.PlayerControllerUpEventName);
                }
            }
        }

        private float GetAngle(Vector3 fromDir, Vector3 toDir)
        {
            float angle = Vector3.SignedAngle(fromDir, toDir, Vector3.up);
            return angle;
        }
    }
}