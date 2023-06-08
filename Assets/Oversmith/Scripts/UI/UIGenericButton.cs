using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;

namespace Oversmith.Scripts.UI
{
    public class UIGenericButton : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent buttonText;
        [SerializeField] private MultiInputButton button = default;

        public UnityAction Clicked = default;
    }
}