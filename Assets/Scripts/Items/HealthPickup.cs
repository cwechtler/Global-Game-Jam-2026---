using UnityEngine;
using UnityEngine.Rendering;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private AttackType maskTypeToActivate;
    public int healAmount = 25;
    public int poisonAmount = 5;
    public Sprite healthSprite;
    public Sprite poisonSprite;

    private PlayerController player;
    private HealthPotionManager manager;
    private SpriteRenderer SpriteRenderer;

    private bool isPoison;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player.IsMaskOn != isPoison)
        {
            SetMaskState(player.IsMaskOn);
        }
    }

    public void Initialize(HealthPotionManager mgr, PlayerController plyr, bool maskActive)
    {
        manager = mgr;
        player = plyr;

        SetMaskState(maskActive);

    }

    public void UpdateMaskState(bool maskOn)
    {
        SetMaskState(maskOn);
    }

    public void SetMaskState (bool maskActive)
    {
        isPoison = maskActive;

        SpriteRenderer.sprite = isPoison ? poisonSprite : healthSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (isPoison)
        {
            player.Health -= poisonAmount;
        }
        else
        {
            player.Health += healAmount;
        }
  
        player.Health = Mathf.Min(player.Health, player.Health);

        manager.ClearPotion();
        Destroy(gameObject);
    }
}
