using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCWarriorMovement : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;

	public GameObject warriorColumn;
	public GameObject warriorsParent;

	public float speed = 1;
	public float secondsToSpawn = 5;

    private bool isMoving = false;
	private Vector3 direction;

	private void Start()
	{
		StartCoroutine(SpawnTimer(secondsToSpawn));
		warriorColumn.transform.position = startPosition;
		warriorsParent.SetActive(false);

		direction = (endPosition - startPosition).normalized;
	}

	private void FixedUpdate()
	{
		if (isMoving)
		{
			warriorColumn.transform.position += direction * speed * Time.deltaTime;
		}

		if (Vector3.Distance(warriorColumn.transform.position, endPosition) < 0.1f)
		{
			isMoving = false;
			warriorColumn.transform.position = startPosition;
			warriorsParent.SetActive(false);

			StartCoroutine(SpawnTimer(secondsToSpawn));
		}
	}

	IEnumerator SpawnTimer(float seconds)
	{
		yield return new WaitForSeconds(seconds);

		warriorsParent.SetActive(true);
		isMoving = true;
	}
}
