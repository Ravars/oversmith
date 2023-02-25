using UnityEngine;

namespace _Developers.Vitor
{
    [CreateAssetMenu(fileName = "newRecipe", menuName = "Items/Recipe")]
    public class Recipe : ScriptableObject
    {
        public GameObject item;
        public Material[] materialsNeeded;
    }
}