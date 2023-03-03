using UnityEngine;

namespace _Developers.Vitor
{
    [CreateAssetMenu(fileName = "newRawMaterial", menuName = "Items/RawMaterial")]
    public class RawMaterialSo : BaseItem
    {
        private RawMaterialSo()
        {
            itemType = ItemType.RawMaterial;
        }
    }
}