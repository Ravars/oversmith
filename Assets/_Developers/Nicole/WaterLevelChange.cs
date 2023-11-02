using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class WaterLevelChange : MonoBehaviour
{
    public GameObject waterCollider1;
    public GameObject waterCollider2;
    public GameObject waterCollider3;
	public GameObject waterObject;

	public GameObject tablesBelow;
	public GameObject tablesAbove;

	public Vector3 downPosition;
	public Vector3 upPosition;

	public Vector3 downPositionCollider;
	public Vector3 upPositionCollider;

	public float speed;
	public float timeToWait;

	private bool isMoving = false;
	private bool isMovingUp = true;
	private bool isMovingDown = false;

	private void Start()
	{
		StartCoroutine(WaterMovement(timeToWait));
	}

	private void FixedUpdate()
	{
		if (isMoving && isMovingUp)
		{
			waterObject.transform.position = Vector3.Lerp(waterObject.transform.position, upPosition, Time.deltaTime * speed);
			waterCollider3.transform.position = Vector3.Lerp(waterCollider3.transform.position, upPositionCollider, Time.deltaTime * speed);

			Debug.Log($"{waterCollider3.transform.position} {upPositionCollider}");

			if (Vector3.Distance(waterObject.transform.position, upPosition) < 0.1f)
			{
				isMoving = false;
				isMovingUp = false;
				isMovingDown = true;

				StartCoroutine(WaterMovement(timeToWait));
			}
		}

		if (isMoving && isMovingDown)
		{
			waterObject.transform.position = Vector3.Lerp(waterObject.transform.position, downPosition, Time.deltaTime * speed);
			waterCollider3.transform.position = Vector3.Lerp(waterCollider3.transform.position, downPositionCollider, Time.deltaTime * speed);

			if (Vector3.Distance(waterObject.transform.position, downPosition) < 0.5f)
			{
				isMoving = false;
				isMovingUp = true;
				isMovingDown = false;

				waterCollider3.SetActive(false);
				waterCollider1.SetActive(true);
				waterCollider3.SetActive(true);

				tablesAbove.SetActive(false);
				tablesBelow.SetActive(true);

				StartCoroutine(WaterMovement(timeToWait));
			}
		}
	}

	IEnumerator WaterMovement(float secondsToChange)
	{
		yield return new WaitForSeconds(secondsToChange);
		isMoving = true;

		if (isMovingUp)
		{
			waterCollider1.SetActive(false);
			waterCollider2.SetActive(false);
			waterCollider3.SetActive(true);

			tablesAbove.SetActive(true);
			tablesBelow.SetActive(false);
		}
	}
}
