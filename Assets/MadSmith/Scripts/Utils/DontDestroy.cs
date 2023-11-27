using System;
using UnityEngine;

namespace MadSmith.Scripts.Utils
{
    public class DontDestroy : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}