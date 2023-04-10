using UnityEngine;
using UnityEngine.UI;

namespace Oversmith.Scripts.Menu
{
    public class PauseMainPanel : MonoBehaviour
    {
        [SerializeField] private Button firstBtn;
        
        public void OnEnable()
        {
            firstBtn.Select();
        }
    }
}