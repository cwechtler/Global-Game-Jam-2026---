using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
	private maskType skillElementType;

	void Start()
	{
		//skillElementType = GetComponentInParent<VacumeHole>().SkillElementType;

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy")) {
			Enemy enemy = collision.GetComponent<Enemy>();
			maskType type = enemy.MaskTypeToActivate;
			if (skillElementType == type) {
				enemy.reduceHealth(100);
			}
		}
	}
}
