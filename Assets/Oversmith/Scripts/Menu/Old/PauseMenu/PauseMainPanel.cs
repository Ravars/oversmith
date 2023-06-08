using UnityEngine;
using UnityEngine.UI;

namespace Oversmith.Scripts.Menu
{
    [RequireComponent(typeof(FadeUI))]
    public class PauseMainPanel : MonoBehaviour
    {
        [SerializeField] private Button firstBtn;
        
        public void OnEnable()
        {
            firstBtn.Select();
        }
    }
}