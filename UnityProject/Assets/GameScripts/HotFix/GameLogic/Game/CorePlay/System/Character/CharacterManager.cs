using System.Collections.Generic;
using GameBase;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameLogic.Game
{
    public partial class CharacterManager : BaseLogicSys<CharacterManager>
    {
        
        public List<CharacterElement> CharacterList = new List<CharacterElement>();
        private ICharacterView m_CharacterView;
        public ICharacterView CharacterView => m_CharacterView;
        public MainCharacter MainChar { get; private set; }

        public override bool OnInit()
        {
            InitRelation();
            return base.OnInit();
        }

        public void InitCharacter()
        {
            CreateMainCharacter();
        }

        public void CreateCharacter(int hexIndex)
        {
            CharacterElement element = new CharacterElement();
            element.Init();
            CharacterList.Add(element);
            XYEvent.GEvent.Fire(this,EventDefine.CreateCharacterEventName, element);
        }

        private void CreateMainCharacter()
        {
            MainChar = new MainCharacter();
            CharacterView?.OnCreateMainCharacter(MainChar);
        }

        public void Inject(ICharacterView view)
        {
            m_CharacterView = view;
        }
        
        
    }
}