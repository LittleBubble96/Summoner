using UnityEngine;

namespace GameLogic.Game
{
    public class SkillWindUpBehavior : SkillBehavior
    {
        public SkillWindUpData Data { get; set; }

        private float _timer;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate(float dt, float currentTime)
        {
            base.OnUpdate(dt, currentTime);
            CharacterElement characterElement = GetCharacter();
            if (characterElement != null && !characterElement.IsDead())
            {
                TargetComponent targetComponent = characterElement.GetComponent<TargetComponent>();
                CharacterElement targetCharacter = targetComponent.GetTargetCharacter();
                if (targetCharacter != null && !targetCharacter.IsDead())
                {
                    Vector3 direction = targetCharacter.GetPosition() - characterElement.GetPosition();
                    direction.y = 0;
                    if (direction.sqrMagnitude > 0.001f)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                        characterElement.ManualControlRotationStart(targetRotation.eulerAngles);
                    }
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            CharacterElement characterElement = GetCharacter();
            if (characterElement != null && !characterElement.IsDead())
            {
                characterElement.CancelManualControlRotation();
            }
        }

        public override void Clear()
        {
            base.Clear();
            _timer = 0f;
            Data = null;
        }
    }
}