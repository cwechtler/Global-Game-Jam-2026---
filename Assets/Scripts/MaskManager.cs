using UnityEngine;

public class MaskManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject maskPickupPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject realWorld;
    [SerializeField] private GameObject cursedWorld;

    [Header("Cooldown")]
    public float cooldownDuration = 10f;
    private float cooldownTimer;
    private bool cooldownActive;

    private GameObject activeMaskPickup;

    // How do we spawn the Mask?
    public void Awake()
    {
        TrySpawnMask();
    }

    private void Update()
    {
        if (!cooldownActive)
            return;

        cooldownTimer -= Time.deltaTime;

        Debug.Log(cooldownTimer);

        if (cooldownTimer <= 0f)
        {
            cooldownActive = false;
            SpawnMask();
        }

        //// Ensure one pickup exists during cooldown
        //if (activeMaskPickup == null)
        //{
        //    SpawnMask();
        //}
    }

    public void StartCooldown()
    {
        cooldownActive = true;
        cooldownTimer = cooldownDuration;
    }

    public bool IsCooldownActive()
    {
        return cooldownActive;
    }


    public void TrySpawnMask()
    {
        if (activeMaskPickup != null)
            return;

        SpawnMask();
    }

    private void SpawnMask()
    {
        if (activeMaskPickup != null)
            return;

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

    public void SwitchMap() 
    {
        if (realWorld.activeInHierarchy) { 
            realWorld.SetActive(false);
            cursedWorld.SetActive(true);
        } else
        {
            cursedWorld.SetActive(false);
            realWorld.SetActive(true);
        }
    }
}
