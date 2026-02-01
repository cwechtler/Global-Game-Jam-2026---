using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ProjectileConfig : SkillConfig
{
    [SerializeField] private GameObject projectileVFX;
    [SerializeField] private float range = 1f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private bool hasEndAnimation = false;

    [Range(.01f, 10f)][SerializeField] private float areaEffect = 6f;

    [Tooltip("The sound to play upon collision.")]
    [SerializeField] private AudioSource projectileCollisionSound;

    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }
    public float Range { get => range; set => range = value; }
    public float AreaEffect { get => areaEffect; set => areaEffect = value; }
    public AudioSource ProjectileCollisionSound { get => projectileCollisionSound; set => projectileCollisionSound = value; }
    public GameObject ProjectileVFX { get => projectileVFX; set => projectileVFX = value; }
    public bool HasEndAnimation { get => hasEndAnimation; set => hasEndAnimation = value; }

    private void OnValidate()
    {
        SetAttackType(AttackType.Projectile);
    }

}
