using System.Collections.Generic;

namespace GameLogic.Game
{
    public partial class CharacterManager
    {
        private Dictionary<CharacterFactionType, Dictionary<CharacterFactionType, FactionRelationType>> _relationDic;
        private void InitRelation()
        {
            _relationDic = new Dictionary<CharacterFactionType, Dictionary<CharacterFactionType, FactionRelationType>>();
            _relationDic[CharacterFactionType.Player] = new Dictionary<CharacterFactionType, FactionRelationType>
            {
                { CharacterFactionType.Player, FactionRelationType.Friendly },
                { CharacterFactionType.Enemy, FactionRelationType.Hostile },
                { CharacterFactionType.Neutral, FactionRelationType.Neutral },
            };
            _relationDic[CharacterFactionType.Enemy] = new Dictionary<CharacterFactionType, FactionRelationType>
            {
                { CharacterFactionType.Player, FactionRelationType.Hostile },
                { CharacterFactionType.Enemy, FactionRelationType.Friendly },
                { CharacterFactionType.Neutral, FactionRelationType.Neutral },
            };
        }

        /// <summary>
        /// 获取关系
        /// </summary>
        public FactionRelationType GetRelation(CharacterFactionType from, CharacterFactionType to)
        {
            if (_relationDic.TryGetValue(from,out var relationTypes))
            {
                if (relationTypes.TryGetValue(to , out var resultRelation))
                {
                    return resultRelation;
                }   
            }
            return FactionRelationType.Neutral;
        }

        public FactionRelationType GetRelation(CharacterElement from, CharacterElement to)
        {
            if (from == null || to == null)
            {
                return FactionRelationType.Friendly;
            }
            return GetRelation(from.FactionType, to.FactionType);
        }
        
        public FactionRelationType GetRelation(ActorInstanceId from, ActorInstanceId to)
        {
            CharacterElement fromCharacter = GetCharacter(from);
            CharacterElement toCharacter = GetCharacter(to);
            return GetRelation(fromCharacter, toCharacter);
        }
    }
}