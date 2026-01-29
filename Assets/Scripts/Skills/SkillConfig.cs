using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum maskType
{
	mask1,
	mask2
    // Add new skill element types here
}

public class SkillConfig : MonoBehaviour
{
	[SerializeField] private Sprite skillImage;
	[SerializeField] private float damage = 100f;
	[SerializeField] private float firingRate = 0.5f;
	[SerializeField] private float coolDownTime = 5f;
	[SerializeField] private bool fireOnce = false;
	[SerializeField] protected maskType maskType;

	public float FireRate { get { return firingRate; } }
	public bool FireOnce { get { return fireOnce; } }
	public maskType MaskType { get { return maskType; } }
	public float CoolDownTime { get => coolDownTime; set => coolDownTime = value; }
	public Sprite SkillImage { get => skillImage;}

	public float GetDamage()
	{
		return damage;
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
