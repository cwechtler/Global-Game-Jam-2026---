using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private maskType skillElementTypeToDestroy;

	[SerializeField] private AudioClip deathClip;
	[SerializeField] private float health = 1f;
	[SerializeField] private int damage = 10;
	[SerializeField] private GameObject expDrop;
	[SerializeField] private int experiencePointValue = 10;
	[SerializeField] private GameObject mainSprite;
	[SerializeField] private GameObject alternateSprite;
    // sprite for alternate image
    // sprite for active image
    // health changes bases on state
    // no ranged atack, they have to touch the player to inflict damage

    public maskType SkillElementTypeToDestroy { get { return skillElementTypeToDestroy; } }
	
	private AIPath aipath;
	private AIDestinationSetter destinationSetter;
	private GameObject player;
	private Rigidbody2D myRigidbody;
    private float damageTimer;
	private bool isDead;
    private bool isStopped;
    private PlayerController playerController;
    private CapsuleCollider2D col;


    void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		aipath = GetComponent<AIPath>();
		myRigidbody = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        playerController = player.GetComponent<PlayerController>();
        destinationSetter = GetComponent<AIDestinationSetter>();
		destinationSetter.target = player.transform;
	}

	void Update()
	{
        bool shouldStop = playerController.ActiveMaskType != skillElementTypeToDestroy;

        if (shouldStop && !isStopped)
		{
            isStopped = true;
            mainSprite.SetActive(false);
			alternateSprite.SetActive(true);
            aipath.canMove = false;
			myRigidbody.gravityScale = 0;
			myRigidbody.velocity = new Vector2(0, 0);
			//myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            myRigidbody.bodyType = RigidbodyType2D.Static;

            var guo = new GraphUpdateObject(col.bounds);
            guo.modifyWalkability = true;
            guo.setWalkability = false;
            AstarPath.active.UpdateGraphs(guo);
        }
        else if (!shouldStop && isStopped)
		{
            isStopped = false;
            mainSprite.SetActive(true);
			alternateSprite.SetActive(false);
			aipath.canMove = true;
			myRigidbody.gravityScale = 1;
			//myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
			myRigidbody.bodyType = RigidbodyType2D.Dynamic;

            var guo = new GraphUpdateObject(col.bounds);
            guo.modifyWalkability = true;
            guo.setWalkability = true; 
            AstarPath.active.UpdateGraphs(guo);
        }

		if (destinationSetter.target != player.transform)
		{
			//FlipDirectionReversed();
		}
		else
		{
			//FlipDirection();
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
			SoundManager.instance.PlayOneShot(deathClip);
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
			if (SkillConfig.MaskType == skillElementTypeToDestroy) {
				reduceHealth(SkillConfig.GetDamage());
			}
		}		
	}

	private void OnParticleCollision(GameObject particle)
	{
		SkillConfig particleParent = particle.GetComponentInParent<SkillConfig>();
		if (particleParent.MaskType == skillElementTypeToDestroy) {
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
