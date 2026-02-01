using UnityEngine;

public class MaskPickup : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite maskOnSprite;
    [SerializeField] private Sprite maskOffSprite;

    private SpriteRenderer spriteRenderer;
    private PlayerController player;
    private MaskManager manager;

    private bool showingMaskOn;
    private bool hasTriggered = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GetComponent<PlayerController>();
    }

    public void Initialize(
        MaskManager mgr,
        PlayerController plyr
    )
    {
        manager = mgr;
        player = plyr;

        UpdateVisual(player.IsMaskOn);
    }

    private void Update()
    {
        if (player == null)
            return;

        // Live update sprite if mask state changes
        if (player.IsMaskOn != showingMaskOn)
        {
            UpdateVisual(player.IsMaskOn);
        }
    }

    private void UpdateVisual(bool maskOn)
    {
        showingMaskOn = maskOn;
        spriteRenderer.sprite =
            maskOn ? maskOnSprite : maskOffSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (hasTriggered)
            return;

        hasTriggered = true;

        if (player.IsMaskOn)
        {
            player.RemoveMask();
            manager.SwitchMap();
        }
        else
        {
            player.PickedupMask();
            manager.SwitchMap();
            manager.StartCooldown();
        }

        manager.ClearMaskPickup();
        Destroy(gameObject);

    }
}
