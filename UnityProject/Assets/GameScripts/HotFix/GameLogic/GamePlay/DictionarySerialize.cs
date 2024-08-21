using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace GameLogic.GamePlay
{
    public class DictionarySerializePlacementGrid:SerializedMonoBehaviour
    {
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        public Dictionary<int,List<PlacementGrid>> ListPlacementGrid_Dic = new Dictionary<int, List<PlacementGrid>>();
    }
}