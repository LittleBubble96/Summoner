namespace GameLogic.Game
{
    public class BaseDamageBuff : BuffItem
    {
        //基础伤害
        private int _baseDamage = 0;
        public override void InitBuff(BuffConfig config, ActorInstanceId belongToActor, ActorInstanceId attachToActor, CommonArgs args)
        {
            base.InitBuff(config, belongToActor, attachToActor, args);
          
            if (args is CommonOneArgs<int> oneArgs)
            {
                _baseDamage = oneArgs.Arg1;
            }
            args.Dispose();
        }

        protected override void OnExecuteBuff()
        {
            base.OnExecuteBuff();
            CharacterElement belongToElement = CharacterManager.Instance.GetCharacter(BelongToActor);
            if (belongToElement != null)
            {
                _baseDamage = belongToElement.PhysicalAttack;
            }
            CharacterElement attachToElement = CharacterManager.Instance.GetCharacter(AttachToActor);
            if (attachToElement != null && !attachToElement.IsDead())
            {
                attachToElement.IncreaseHp(_baseDamage,DamageSourceType.PhysicalAttack);
            }
        }
    }
}