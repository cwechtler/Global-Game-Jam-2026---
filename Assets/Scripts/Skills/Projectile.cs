using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Projectile : SkillConfig
{

    [SerializeField] private float range = 1f;
    [SerializeField] private float projectileSpeed = 10f;
    [Space]
    [Tooltip("The particle system to play.")]
    [SerializeField] private ParticleSystem ProjectileParticleSystem;
    [Tooltip("The particle system to play upon collision.")]
    [SerializeField] private ParticleSystem ProjectileExplosionParticleSystem;
    [Range(.01f, 10f)][SerializeField] private float areaEffect = 6f;

    [Tooltip("The sound to play upon collision.")]
    [SerializeField] private AudioSource ProjectileCollisionSound;

    private bool stop = false;
    ParticleSystem.MainModule main;

    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }
    public float Range { get => range; set => range = value; }

    void Start()
    {
        transform.up = GetComponent<Rigidbody2D>().velocity;
        if (ProjectileParticleSystem)
        {
            ProjectileParticleSystem.Play();
        }
        main = ProjectileParticleSystem.main;
        StartCoroutine(DestroySkill(Range));
    }

    void Update()
    {
        
    }

    private IEnumerator DestroySkill(float skillDuration)
    {
        yield return new WaitForSeconds(skillDuration);
        stop = true;
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, areaEffect);
    }
}
