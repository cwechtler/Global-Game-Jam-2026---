using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
	mask1,
	mask2,
    Cone45,
    Radial,
    ExpandingRadius,
    Projectile
    // Add new skill element types here
}

public class SkillConfig : MonoBehaviour
{
	[SerializeField] private Sprite skillImage;
	[SerializeField] private float damage = 100f;
	[SerializeField] private float firingRate = 0.5f;
	[SerializeField] private float coolDownTime = 5f;
	[SerializeField] private bool fireOnce = false;
	[SerializeField] private AttackType attackType;

	public float FireRate { get { return firingRate; } }
	public bool FireOnce { get { return fireOnce; } }
	public AttackType AttackType { get { return attackType; } }
	public float CoolDownTime { get => coolDownTime; set => coolDownTime = value; }
	public Sprite SkillImage { get => skillImage;}

	public float GetDamage()
	{
		return damage;
	}

    protected void SetAttackType(AttackType type)
    {
        attackType = type;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		Destroy(gameObject);
	}

	protected virtual void OnCollisionEnter2D(Collision2D collision)
	{
		Destroy(gameObject);
	}
}
