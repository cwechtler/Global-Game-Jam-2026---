using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSkill : SkillConfig
{
    [Header("Radial Attack Settings")]
    [SerializeField] private GameObject radialVFX;
    [SerializeField] private float radius = 2f;
    //[SerializeField] private bool useExpandingEffect = false;
    //[SerializeField] private float expandSpeed = 5f;
    //[SerializeField] private float maxRadius = 4f;

    public float Radius => radius;
    public readonly AttackType maskType = AttackType.Radial;
    //public bool UseExpandingEffect => useExpandingEffect;
    //public float ExpandSpeed => expandSpeed;
    //public float MaxRadius => maxRadius;

    public GameObject RadialVFX { get => radialVFX; }

    private void OnValidate()
    {
        SetAttackType(AttackType.Radial);
    }
}
