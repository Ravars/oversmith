using MadSmith.Scripts.Managers;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

namespace MadSmith.Scripts.Utils
{
    public abstract class PersistentNetworkSingleton<T> : NetworkSingleton<T> where T : NetworkSingleton<T>
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}