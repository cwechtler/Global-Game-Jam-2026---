using UnityEngine;

public class MaskManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject maskPickupPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Worlds")]
    [SerializeField] private GameObject realWorld;
    [SerializeField] private GameObject cursedWorld;
    
    [Header("Animators")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController maskOverride;

    [Header("Cooldown")]
    public float cooldownDuration = 10f;
    private float cooldownTimer;
    private bool cooldownActive;

    private GameObject activeMaskPickup;
    private RuntimeAnimatorController baseController;

    // How do we spawn the Mask?
    public void Awake()
    {
        baseController = animator.runtimeAnimatorController;
        TrySpawnMask();
    }

    private void Update()
    {
        if (!cooldownActive)
            return;

        cooldownTimer -= Time.deltaTime;

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
            animator.runtimeAnimatorController = maskOverride;
        } else
        {
            animator.runtimeAnimatorController = baseController;
            cursedWorld.SetActive(false);
            realWorld.SetActive(true); 
        }
    }
}
