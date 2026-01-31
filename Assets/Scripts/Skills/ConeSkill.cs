using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeSkill : SkillConfig
{
    [Header("Cone Attack Settings")]
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private float angle = 45f;   // half‑angle of the cone
    [SerializeField] private bool useAnimationHitTiming = false;

    public float Radius => radius;
    public float Angle => angle;
    public bool UseAnimationHitTiming => useAnimationHitTiming;
    public float HitDelay = 0.12f;
}
