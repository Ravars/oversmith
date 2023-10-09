using MadSmith.Scripts.Items;
using UnityEngine;

namespace _Developers.Nicole.ScriptableObjects.Data_Structures
{
    [CreateAssetMenu(fileName = "newLevelConfigItems", menuName = "Items/Level Config Items")]
    public class LevelConfigItems : ScriptableObject
    {
        // public List<ItemStruct> Items;
        public BaseItem[] itemsToDelivery;
        public BaseItem[] itemsNoAllowed;
    }
}
