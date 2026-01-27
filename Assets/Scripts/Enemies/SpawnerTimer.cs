using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTimer : MonoBehaviour
{
	[SerializeField] private float timeToSpawn;
	[SerializeField] private GameObject spawnerGO;

	void Start()
	{
		StartCoroutine(SpawnAfterTime(timeToSpawn));
	}

	private IEnumerator SpawnAfterTime(float time) {
		yield return new WaitForSeconds(time);
        spawnerGO.gameObject.SetActive(true);
	}
}
