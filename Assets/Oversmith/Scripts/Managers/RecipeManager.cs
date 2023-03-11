using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Developers.Vitor
{
    public class RecipeManager : MonoBehaviour
    {
        // A ideia aqui é ler os arquivos de Scriptable Object da pasta Resource.
        [SerializeField] private List<BaseItem> recipes;
        // public MaterialSo[] test;

        // private void Start()
        // {
        //     Debug.Log(TryGetRecipe(test));
        // }

        // public BaseItem TryGetRecipe(MaterialSo[] materials)
        // {
        //     if (materials.Length < 2) return null;
        //
        //     foreach (var recipe in recipes)
        //     {
        //         var hasAllItem = true;
        //         foreach (var material in materials)
        //         {
        //             if (!recipe.materialsNeeded.Contains(material))
        //             {
        //                 hasAllItem = false;
        //                 break;
        //             }
        //         }
        //
        //         if (hasAllItem)
        //         {
        //             return recipe;
        //         }
        //     }
        //     return null;
        // }

    }
}