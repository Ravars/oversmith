using UnityEngine;

namespace _Developers.Vitor
{
    [CreateAssetMenu(fileName = "Feedstock", menuName = "Items/feedstock", order = 0)]
    public class Feedstock : ScriptableObject
    {
        public string feedstockName;
        public BaseItem prefab;
    
        // public CraftingTable[] craftingTables;
        // public
    }
}