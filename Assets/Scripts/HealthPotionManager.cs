using UnityEngine;

public class HealthPotionManager : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;
    public GameObject healthPotionPrefab;
    public Transform[] spawnPoints;

    [Header("Spawn Rules")]
    [Range(0f, 1f)]
    public float spawnChance = 0.25f;

    //Craig fix this
    public bool maskActive;

    private GameObject activePotion;
    private int lastHealth;

    private void Start()
    {
        lastHealth = player.Health;
    }

    private void Update()
    {
        // Detect health change
        if (player.Health == lastHealth)
            return;

        // Only respond to DAMAGE
        if (player.Health < lastHealth && activePotion == null)
        {
            if (Random.value <= spawnChance)
                SpawnPotion();
        }

        lastHealth = player.Health;
    }

    private void SpawnPotion()
    {
        if (spawnPoints.Length == 0)
            return;

        Transform spawn =
            spawnPoints[Random.Range(0, spawnPoints.Length)];

        activePotion = Instantiate(
            healthPotionPrefab,
            spawn.position,
            Quaternion.identity
        );

        bool maskActive = player.IsMaskOn;

        activePotion
            .GetComponent<HealthPickup>()
            .Initialize(this, player, maskActive);
    }

    public void ClearPotion()
    {
        activePotion = null;
    }
}
