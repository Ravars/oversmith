using MadSmith.Scripts.Managers;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

namespace MadSmith.Scripts.Utils
{
    public abstract class NetworkSingleton<T> : NetworkBehaviour where T : NetworkSingleton<T>
    {
        public static T Instance { get; protected set; }
        public static bool InstanceExists => !ReferenceEquals(Instance, null);

        protected virtual void Awake()
        {
            if (InstanceExists)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = (T)this;
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}