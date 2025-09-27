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
    }
}