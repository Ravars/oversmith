using MadSmith.Scripts.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace MadSmith.Scripts.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("Screens")]
        [SerializeField] private GameObject MainScr;    // Main screen: list of buttons
        [SerializeField] private GameObject ConfigScr;  // Configuration screen
        
    }
}