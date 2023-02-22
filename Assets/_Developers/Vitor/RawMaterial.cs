using UnityEngine;

namespace _Developers.Vitor
{
    [CreateAssetMenu(fileName = "newRawMaterial", menuName = "Items/RawMaterial")]
    public class RawMaterial : BaseItem
    {
        private RawMaterial()
        {
            itemType = ItemType.RawMaterial;
        }
    }
}