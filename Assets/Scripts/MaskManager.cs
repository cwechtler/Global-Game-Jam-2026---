using UnityEngine;

public class MaskManager : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;
    public GameObject maskPickupPrefab;
    public Transform[] spawnPoints;

    private GameObject activeMaskPickup;

    // How do we spawn the Mask?
    public void Awake()
    {
        TrySpawnMask();
    }

    public void TrySpawnMask()
    {
        if (activeMaskPickup != null)
            return;

        SpawnMask();
    }

    private void SpawnMask()
    {
        if (spawnPoints.Length == 0)
            return;

        Transform spawn =
            spawnPoints[Random.Range(0, spawnPoints.Length)];

        activeMaskPickup = Instantiate(
            maskPickupPrefab,
            spawn.position,
            Quaternion.identity
        );

        activeMaskPickup
            .GetComponent<MaskPickup>()
            .Initialize(this, player);
    }

    public void ClearMaskPickup()
    {
        activeMaskPickup = null;
    }
}
