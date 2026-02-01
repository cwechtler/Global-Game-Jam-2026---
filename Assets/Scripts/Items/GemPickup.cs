using UnityEngine;

public class GemPickup : MonoBehaviour
{
    private int amount;

    public void Init(Sprite sprite, int value)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        amount = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // TODO: Player currency
        // Somethin.Instance.AddGems(amount);

        Destroy(gameObject);
    }
}