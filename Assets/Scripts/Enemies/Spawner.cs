using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[Tooltip("Spawns random between 2 seconds and 'Max Wait To Spawn' seconds")]
	[Range(0, 10)] [SerializeField] private int maxWaitToSpawn = 5;
	[SerializeField] private GameObject[] enemyPrefabs;

	private PlayerController playerController;
	private Enemy enemy;


    void Start()
	{
		playerController = FindObjectOfType<PlayerController>();
        StartCoroutine(SpawnEnemies());
	}

    private bool IsVisibleToCamera()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPos.z < 0)
            return false;

        return viewportPos.x >= 0f && viewportPos.x <= 1f &&
               viewportPos.y >= 0f && viewportPos.y <= 1f;
    }



    private IEnumerator SpawnEnemies()
	{
		while (true) {
			if (!IsVisibleToCamera())
			{
                GameObject prefabToSpawn = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];
                Enemy enemyComponent = prefabToSpawn.GetComponent<Enemy>();
				//if (enemyComponent != null && enemyComponent.MaskTypeToActivate == playerController.IsMaskOn)
				//{
                    GameObject enemy = Instantiate(prefabToSpawn, transform.position, Quaternion.identity) as GameObject;
					enemy.transform.SetParent(this.transform);
				//}
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(0, maxWaitToSpawn));
		}
	}
}
