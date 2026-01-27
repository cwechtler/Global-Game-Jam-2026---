using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	[SerializeField] private GameObject inventoryPrefab;

	private bool pickedUp;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !pickedUp) {
			pickedUp = true;
			collision.GetComponent<PlayerController>().AddToInventory(inventoryPrefab);
			GameObject.Destroy(this.gameObject);
		}
	}
}
