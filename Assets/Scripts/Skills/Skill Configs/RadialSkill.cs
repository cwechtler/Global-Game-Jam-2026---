using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSkill : SkillConfig
{
    [Header("Radial Attack Settings")]
    [SerializeField] private GameObject radialVFX;
    [SerializeField] private float radius = 2f;
    [Tooltip("Can not be more than coolDownTime")]
    [SerializeField] private float skillDuration = 1f;
    [SerializeField] private float damageInterval = 0.2f;


    public float Radius => radius;
    public readonly AttackType maskType = AttackType.Radial;

    public GameObject RadialVFX { get => radialVFX; }
    public float Duration { get => skillDuration; set => skillDuration = value; }
    public float TickRate { get => damageInterval; set => damageInterval = value; }

    private void OnValidate()
    {
        SetAttackType(AttackType.Radial);
        if (skillDuration > coolDownTime)
            skillDuration = coolDownTime;
    }
}
