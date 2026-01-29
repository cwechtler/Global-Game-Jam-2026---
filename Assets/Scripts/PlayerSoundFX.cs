using UnityEngine;

public class PlayerSoundFX : MonoBehaviour
{
    // Called directly from Animation Events
    public void WalkStep()
    {
        SoundManager.instance.PlayWalkClip();
    }

    public void RunStep()
    {
        SoundManager.instance.PlayRunClip();
    }

    public void Death()
    {
        SoundManager.instance.PlayDeathClip();
    }

    public void SwordSwipe()
    {
        //SoundManager.instance.PlayAttackClips();
    }

    public void Hurt()
    {
        SoundManager.instance.PlayHurtClip();
    }
}