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
	public AudioSource[] audios;
	public AudioSource horn = null;

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
			foreach (var source in audios)
			{
				source.Stop();
			}
			warriorColumn.transform.position = startPosition;
			warriorsParent.SetActive(false);

			StartCoroutine(SpawnTimer(secondsToSpawn));
		}
	}

	IEnumerator SpawnTimer(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		if (horn != null)
		{
			horn.Play();
		}
		yield return new WaitForSeconds(2);
		warriorsParent.SetActive(true);
		isMoving = true;
		
		foreach (var source in audios)
		{
			source.time = 2.9f + Random.Range(-0.5f, 1.0f);
			source.Play();
		}
	}
}
