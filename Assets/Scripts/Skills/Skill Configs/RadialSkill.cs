using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSkill : SkillConfig
{
    [Header("Radial Attack Settings")]
    [SerializeField] private GameObject radialVFX;
    [SerializeField] private float radius = 2f;

    public float Radius => radius;
    public readonly AttackType maskType = AttackType.Radial;

    public GameObject RadialVFX { get => radialVFX; }

    private void OnValidate()
    {
        SetAttackType(AttackType.Radial);
    }
}
