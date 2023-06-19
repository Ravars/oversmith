using Oversmith.Scripts.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Oversmith.Scripts.Systems.Settings
{
    public class UIEndGame : MonoBehaviour
    {
        public event UnityAction Continued = default;

        public void Setup(int finalScore)
        {
            
        }

        public void ContinueButton()
        {
            Continued?.Invoke();
        }
        
        // public bu
    }
}