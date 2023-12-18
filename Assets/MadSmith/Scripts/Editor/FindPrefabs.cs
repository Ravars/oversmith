using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class FindPrefabsWithScript : EditorWindow
{
    [MenuItem("Custom/Find Prefabs With Script")]
    public static void FindPrefabs()
    {
        string scriptName = "BaseItem"; // Substitua pelo nome do seu script

        string[] guids = AssetDatabase.FindAssets("t:Prefab");

        List<GameObject> prefabsWithScript = new List<GameObject>();

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

            if (obj != null)
            {
                PrefabType prefabType = PrefabUtility.GetPrefabType(obj);

                if (prefabType == PrefabType.Prefab || prefabType == PrefabType.PrefabInstance || prefabType == PrefabType.DisconnectedPrefabInstance)
                {
                    var components = obj.GetComponentsInChildren<MonoBehaviour>(true);
                    foreach (var component in components)
                    {
                        if (component != null && component.GetType().Name == scriptName)
                        {
                            prefabsWithScript.Add(obj);
                            break;
                        }
                    }
                }
            }
        }

        if (prefabsWithScript.Count > 0)
        {
            Selection.objects = prefabsWithScript.ToArray();
            //Debug.Log(prefabsWithScript.Count + " prefab(s) com o script '" + scriptName + "' selecionado(s).");
        }
        else
        {
            //Debug.Log("Nenhum prefab encontrado com o script '" + scriptName + "'.");
        }
    }
}