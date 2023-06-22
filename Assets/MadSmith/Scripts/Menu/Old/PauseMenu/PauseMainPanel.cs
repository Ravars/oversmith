using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.Menu.Old.PauseMenu
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