using System;
using System.Collections;
using Oversmith.Scripts.Menu;
using Oversmith.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Oversmith.Scripts.Managers
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        public int characterIndex = 0;
        public GameObject[] charactersPrefabs;
    }
}