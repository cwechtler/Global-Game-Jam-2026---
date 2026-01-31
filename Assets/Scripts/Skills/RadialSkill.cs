using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSkill : SkillConfig
{
    [Header("Radial Attack Settings")]
    [SerializeField] private float radius = 2f;
    [SerializeField] private bool useExpandingEffect = false;
    [SerializeField] private float expandSpeed = 5f;
    [SerializeField] private float maxRadius = 4f;

    public float Radius => radius;
    public bool UseExpandingEffect => useExpandingEffect;
    public float ExpandSpeed => expandSpeed;
    public float MaxRadius => maxRadius;
}
