using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGemDrop : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject gemPrefab;

    [Header("Gem Table")]
    [SerializeField] private GemData[] gems;

    private bool dropped = false;

    private void OnEnable()
    {
        enemy.OnDeath += DropGem;
    }

    private void OnDisable()
    {
        enemy.OnDeath -= DropGem;
    }

    private void DropGem()
    {
        if (dropped) return;
        dropped = true;

        GemData selectedGem = GetRandomGem();

        GameObject gem = Instantiate(
            gemPrefab,
            transform.position,
            Quaternion.identity
        );

        gem.GetComponent<GemPickup>()
            .Init(selectedGem.sprite, selectedGem.amount);
    }

    private GemData GetRandomGem()
    {
        float totalWeight = 0f;

        foreach (GemData gem in gems)
            totalWeight += gem.rarity;

        float roll = Random.Range(0, totalWeight);

        foreach (GemData gem in gems)
        {
            if (roll < gem.rarity)
                return gem;

            roll -= gem.rarity;
        }

        return gems[0]; // fallback
    }
}

[System.Serializable]
public class GemData
{
    public string gemName;
    public Sprite sprite;
    public int amount;
    public float rarity; // weight
}