using UnityEngine;

public class PlayerSoundFX : MonoBehaviour
{
    [SerializeField] float minStepInterval = 0.12f;

    private Animator animator;
    private PlayerController player;
    private float lastStepTime = 0f;


    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<PlayerController>();
    }           

    // Called directly from Animation Events
    public void WalkStep()
    {
        SoundManager.instance.PlayWalkClip();
    }

    public void RunStep()
    {
        if (!IsStillMoving())
            return;

        // Prevent double-triggering
        if (Time.time - lastStepTime < minStepInterval)
            return;

        lastStepTime = Time.time;

        SoundManager.instance.PlayRunClip();
    }

    public void Death()
    {
        SoundManager.instance.PlayDeathClip();
    }

    public void SwordSwipe()
    {
        SoundManager.instance.PlayAttackClip();
    }

    public void Hurt()
    {
        SoundManager.instance.PlayHurtClip();
    }

    private bool IsStillMoving()
    {
        return player.currentDirection != PlayerController.MoveDir.None;
    }
}