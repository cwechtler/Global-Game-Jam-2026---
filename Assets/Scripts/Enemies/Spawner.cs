using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[Tooltip("Spawns random between 2 seconds and 'Max Wait To Spawn' seconds")]
	[Range(2, 10)] [SerializeField] private int maxWaitToSpawn = 5;
	[SerializeField] private GameObject[] enemyPrefabs;

	void Start()
	{
		StartCoroutine(SpawnEnemies());
	}

	private IEnumerator SpawnEnemies()
	{
		while (true) {
			GameObject prefabToSpawn = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];
			GameObject enemy = Instantiate(prefabToSpawn, transform.position, Quaternion.identity) as GameObject;
			enemy.transform.SetParent(this.transform);
			yield return new WaitForSeconds(UnityEngine.Random.Range(2, maxWaitToSpawn));
		}
	}
}
