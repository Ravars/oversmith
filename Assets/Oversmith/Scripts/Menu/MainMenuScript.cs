using UnityEngine;
using UnityEngine.InputSystem;

namespace Oversmith.Scripts.Menu
{
	public class MainMenuScript : MonoBehaviour
	{
		delegate void Ends();

		public GameObject startScr;
		public GameObject menuScr;

		private PlayerInput input;

		// Start is called before the first frame update
		void Start()
		{
			input = GetComponent<PlayerInput>();
			startScr.SetActive(true);
		}

		public void SwitchToMainScreen()
		{
			startScr.SetActive(false);
			menuScr.SetActive(true);
		}
	}
}
