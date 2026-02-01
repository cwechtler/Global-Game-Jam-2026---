using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalConfig : SkillConfig
{
    [Header("Radial Attack Settings")]
    [SerializeField] private GameObject radialVFX;
    [SerializeField] private float triggerRadius = 2f;
    [Tooltip("Can not be more than coolDownTime")]
    [SerializeField] private float skillDuration = 1f;


    public readonly AttackType maskType = AttackType.Radial;

    public GameObject RadialVFX { get => radialVFX; }
    public float Duration { get => skillDuration; set => skillDuration = value; }
    public float TriggerRadius { get => triggerRadius; set => triggerRadius = value; }

    private void OnValidate()
    {
        SetAttackType(AttackType.Orbital);
    }
}
