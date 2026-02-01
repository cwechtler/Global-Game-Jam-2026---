using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingSkill : SkillConfig
{
    [Header("Expanding AoE Settings")]
    [SerializeField] private float startRadius = 0.1f;
    [SerializeField] private float maxRadius = 4f;
    [SerializeField] private float expandSpeed = 5f;

    public float StartRadius => startRadius;
    public float MaxRadius => maxRadius;
    public float ExpandSpeed => expandSpeed;

    private void OnValidate()
    {
        SetAttackType(AttackType.ExpandingRadius);
    }
}
