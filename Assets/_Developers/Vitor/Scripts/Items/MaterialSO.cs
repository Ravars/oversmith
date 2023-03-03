using UnityEngine;

namespace _Developers.Vitor
{
    [CreateAssetMenu(fileName = "newMaterial", menuName = "Items/Material")]
    public class MaterialSo : BaseItem
    {
        private MaterialSo()
        {
            itemType = ItemType.Material;
        }
    }
}