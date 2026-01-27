using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
	private skillElementType skillElementType;

	void Start()
	{
		//skillElementType = GetComponentInParent<VacumeHole>().SkillElementType;

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy")) {
			Enemy enemy = collision.GetComponent<Enemy>();
			skillElementType type = enemy.SkillElementTypeToDestroy;
			if (skillElementType == type) {
				enemy.reduceHealth(100);
			}
		}
	}
}
