using UnityEngine;

namespace _Developers.Vitor
{
    [CreateAssetMenu(fileName = "newRecipe", menuName = "Items/Recipe")]
    public class RecipeSo : ScriptableObject
    {
        public BaseItem itemGenerated;
        public MaterialSo[] materialsNeeded;
    }
}