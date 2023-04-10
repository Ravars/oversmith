using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Oversmith.Scripts.Menu
{
	public class PauserPlayer : MonoBehaviour
	{
		public UnityEvent<PlayerInput> Pause;
		

		public PlayerInput input;

		public void OnPause()
		{
			Pause.Invoke(input);
		}
	}
}