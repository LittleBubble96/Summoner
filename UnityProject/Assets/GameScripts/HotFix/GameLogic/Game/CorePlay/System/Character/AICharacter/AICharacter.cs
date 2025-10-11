using GameConfig.role;
using GameProto;

namespace GameLogic.Game
{
    public class AICharacter : CharacterElement
    {
        public Role RoleConfig { get; set; }
        public BehaviorTree BehaviorTree { get; set; }

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
                BehaviorTree.Init(new BTGenInfo(RoleConfig.Ai));
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
        }

        public override void DoUpdate(float dt)
        {
            base.DoUpdate(dt);
            if (BehaviorTree != null)
            {
                BehaviorTree.Execute(dt);
            }
        }

        public override void Clear()
        {
            base.Clear();
            RoleConfig = null;
        }
    }
}