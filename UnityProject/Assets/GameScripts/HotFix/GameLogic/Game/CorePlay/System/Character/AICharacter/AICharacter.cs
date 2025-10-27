using GameConfig.role;
using GameProto;
using UnityEngine;

namespace GameLogic.Game
{
    public class AICharacter : CharacterElement
    {
        protected BehaviorTree BehaviorTree { get; set; }
        
        //导航位置
        public Vector3 NavTargetPosition { get; set; }
        public bool IsNavToTarget { get; set; }
        public float NavToTargetRemainDistance { get; set; }

        public bool NavUpdateRotation => !IsManualControlRotation; //寻路是否控制旋转
        //死亡倒计时
        private float _deathTimer;

        protected override void OnInit(CommonArgs args)
        {
            base.OnInit(args);
            if (args is CommonOneArgs<int> commonArgs)
            {
                if (!ConfigSystem.Instance.Tables.TbRole.DataMap.TryGetValue(commonArgs.Arg1, out var role))
                {
                    return;
                }
                RoleConfig = role;
                BehaviorTree = new BehaviorTree();
                int aiId = FactionType == CharacterFactionType.Enemy ? RoleConfig.EmemyAi : RoleConfig.Ai;
                BehaviorTree.Init(new BTGenInfo(aiId),this);
            }
        }

        protected override void OnInitAttribute()
        {
            base.OnInitAttribute();
            if (RoleConfig == null)
            {
                return;
            }
            SetHp(RoleConfig.MaxHp,DamageSourceType.None);
            SetMoveSpeed(RoleConfig.MoveSpeed);
            SetPhysicalAttack(RoleConfig.BasePhysicalAttack);
            SetAttackDistance(RoleConfig.AttackDistance);
        }

        public override void DoUpdate(float dt)
        {
            base.DoUpdate(dt);
            // 处理死亡倒计时
            if (_deathTimer > 0)
            {
                _deathTimer -= dt;
                if (_deathTimer <= 0)
                {
                    DeathComplete();
                }
            }
            if (BehaviorTree != null)
            {
                BehaviorTree.Execute(dt);
            }
        }

        public override void Death()
        {
            base.Death();
            _deathTimer = RoleConfig.DeathTime;
        }

        //开始导航
        public void NavToTarget(Vector3 targetPosition,float remainDistance = 0.1f)
        {
            NavTargetPosition = targetPosition;
            IsNavToTarget = true;
            NavToTargetRemainDistance = remainDistance;
        }

        //停止导航
        public void NavToStop()
        {
            IsNavToTarget = false;
            NavToTargetRemainDistance = 0;
            NavTargetPosition = Vector3.zero;
        }
        
        public override void Clear()
        {
            base.Clear();
            RoleConfig = null;
            _deathTimer = 0;
        }
    }
}