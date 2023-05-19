using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Oversmith.Scripts.Menu
{
    public abstract class MenuCanvas : MonoBehaviour
    {
        public UnityEvent<System.Action> OnReturn;
        public abstract void Begin();
    }
}

