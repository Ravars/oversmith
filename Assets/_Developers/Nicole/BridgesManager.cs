using MadSmith.Scripts.Animations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgesManager : MonoBehaviour
{
    public BridgeAnimation[] bridgeAnimations;

    public float minTimeToFall;
    public float maxTimeToFall;

	private void Start()
	{
		Invoke("ChooseBridgeAndFall", Random.Range(minTimeToFall, maxTimeToFall));
	}

	private void ChooseBridgeAndFall()
	{
		int count = 0;
		
		int i = Random.Range(0, bridgeAnimations.Length);

		while (bridgeAnimations[i].hasFallen && count < bridgeAnimations.Length)
		{
			i = (i + 1) % bridgeAnimations.Length;
			count++;
		}

		if (!bridgeAnimations[i].hasFallen)
		{
			bridgeAnimations[i].BridgeFall();
		}

		Invoke("ChooseBridgeAndFall", Random.Range(minTimeToFall, maxTimeToFall));

	}
}
