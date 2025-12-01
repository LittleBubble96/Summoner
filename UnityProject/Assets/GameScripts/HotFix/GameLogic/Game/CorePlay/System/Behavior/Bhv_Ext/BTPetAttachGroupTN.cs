namespace GameLogic.Game
{
    public class BTPetAttachGroupTN: BTTaskNode
    {
        //0: 主角 1: 其他
        private int _targetType;
        protected override void OnRecycle()
        {
            
        }

        protected override void OnBegin()
        {
            if (_targetType == 0)
            {
                AttachMain();
            }
        }

        protected override void OnEnd()
        {
            
        }

        protected override BtNodeResult OnExecute(float deltaTime)
        {
            return BtNodeResult.Succeeded;
        }

        protected override void OnParseParams(string[] args)
        {
            if (args.Length >= 1)
            {
                _targetType = int.Parse(args[0]);
            }
        }

        private void AttachMain()
        {
            CharacterElement main = CharacterManager.Instance.MainCharacter();
            if (main != null && !main.IsDead())
            {
                CharacterManager.Instance.AttachCharacter(behaviorTree.GetOwnerCharacter().ActorInstanceId , main.ActorInstanceId);
            }
        }
    }
}