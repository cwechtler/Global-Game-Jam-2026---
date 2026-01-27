using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private skillElementType skillElementTypeToDestroy;
	[SerializeField] private AudioClip deathClip;
	[SerializeField] private float health = 1f;
	[SerializeField] private int damage = 10;
	[SerializeField] private GameObject expDrop;
	[SerializeField] private int experiencePointValue = 10;

	public skillElementType SkillElementTypeToDestroy { get { return skillElementTypeToDestroy; } }
	
	private AIPath aipath;
	private AIDestinationSetter destinationSetter;
	private GameObject player;
	private float damageTimer;
	private bool isDead;


	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		aipath = GetComponent<AIPath>();
		destinationSetter = GetComponent<AIDestinationSetter>();
		destinationSetter.target = player.transform;
	}

	void Update()
	{
		if (destinationSetter.target != player.transform) {
			FlipDirectionReversed();
		}
		else {
			FlipDirection();
		}

		if (damageTimer > 0) {
			damageTimer -= Time.deltaTime;
		}

		if (destinationSetter.target == null) {
			destinationSetter.target = player.transform;
			aipath.maxSpeed = 4f;
			gameObject.layer = 9;
		}
	}

	public void reduceHealth(float damage) {
		health -= damage;
		if (health <= 0 && !isDead) {
			isDead = true;
			Destroy(gameObject);
			SoundManager.instance.EnemyDeathSound(deathClip);
			GameController.instance.EnemiesKilled++;
			GameController.instance.AddEnemyType(skillElementTypeToDestroy);

			////Drop Experience - MT
			//GameObject drop = Instantiate(expDrop, transform.position, transform.rotation) as GameObject;
			//drop.GetComponent<Experience>().ExperiencePointsWorth = experiencePointValue;
			//GameObject DropContainer = GameObject.FindGameObjectWithTag("DropContainer");
			//drop.transform.SetParent(DropContainer.transform);
		}
	}

	private void DamagePlayer() {
		damageTimer = 1;
		player.GetComponent<PlayerController>().ReduceHealth(damage);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player")) {
			if (damageTimer <= 0) {
				DamagePlayer();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Skill")) {
			SkillConfig SkillConfig = collision.GetComponentInParent<SkillConfig>();
			if (SkillConfig.SkillElementType == skillElementTypeToDestroy) {
				reduceHealth(SkillConfig.GetDamage());
			}
		}		
	}

	private void OnParticleCollision(GameObject particle)
	{
		SkillConfig particleParent = particle.GetComponentInParent<SkillConfig>();
		if (particleParent.SkillElementType == skillElementTypeToDestroy) {
			reduceHealth(particleParent.GetDamage());
		}
	}

	private void FlipDirection()
	{
		if (aipath.desiredVelocity.x >= 0.01f) {
			transform.localScale = new Vector3(1f, 1f, 0);
		}
		else if(aipath.desiredVelocity.x <= -0.01f){
			transform.localScale = new Vector3(-1f, 1f, 0);
		}
	}
	private void FlipDirectionReversed()
	{
		if (aipath.desiredVelocity.x >= 0.01f) {
			transform.localScale = new Vector3(-1f, 1f, 0);
		}
		else if (aipath.desiredVelocity.x <= -0.01f) {
			transform.localScale = new Vector3(1f, 1f, 0);
		}
	}
}
