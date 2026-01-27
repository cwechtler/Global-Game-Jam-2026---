using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
	public int ExperiencePointsWorth { get; set; }

	private bool collected = false;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && !collected)
		{
			collected = true;
			collision.GetComponent<PlayerController>().ExperiencePoints += ExperiencePointsWorth;
			Destroy(transform.gameObject);
		}
	}
}
