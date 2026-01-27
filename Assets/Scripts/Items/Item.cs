using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
	Key
}

public class Item : MonoBehaviour
{
	[SerializeField] private ItemType itemType;
	[SerializeField] private GameObject inventoryPrefab;

	private bool pickedUp;


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !pickedUp) {
			pickedUp = true;
			collision.GetComponent<PlayerController>().AddToInventory(inventoryPrefab);
			GameObject.Destroy(this.gameObject);
		}
	}
}
