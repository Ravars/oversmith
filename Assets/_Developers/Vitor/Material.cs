using UnityEngine;

namespace _Developers.Vitor
{
    [CreateAssetMenu(fileName = "newMaterial", menuName = "Items/Material")]
    public class Material : BaseItem
    {
        private Material()
        {
            itemType = ItemType.Material;
        }
    }
}