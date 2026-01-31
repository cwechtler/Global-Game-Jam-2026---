using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private maskType maskTypeToActivate;

	[SerializeField] private AudioClip deathClip;
	[Space]
    [SerializeField] private float health = 1f;
	[SerializeField] private int damage = 10;
    [SerializeField] private float aipathMaxSpeed = 4f;
	[Space]
    [SerializeField] private GameObject expDrop;
	[SerializeField] private int experiencePointValue = 10;
	[SerializeField] private GameObject mainSprite;
	[SerializeField] private GameObject alternateSprite;

    // health changes based on state
    // no ranged atack, they have to touch the player to inflict damage

    public maskType MaskTypeToActivate { get { return maskTypeToActivate; } }
	
	private AIPath aipath;
	private AIDestinationSetter destinationSetter;
	private GameObject player;
	private Rigidbody2D myRigidbody;
    private float damageTimer;
	private bool isDead;
    private bool isStopped;
    private PlayerController playerController;
    private CapsuleCollider2D capsuleCollider;

    void Start()
	{
        player = GameObject.FindGameObjectWithTag("Player");
		aipath = GetComponent<AIPath>();
        aipath.maxSpeed = aipathMaxSpeed;
        myRigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerController = player.GetComponent<PlayerController>();
        destinationSetter = GetComponent<AIDestinationSetter>();
		destinationSetter.target = player.transform;
	}

	void Update()
	{
        bool shouldStop = playerController.ActiveMaskType != maskTypeToActivate;

        if (shouldStop && !isStopped)
        {
            StopEnemy();
        }
        else if (!shouldStop && isStopped)
        {
            startEnemy();
        }

        if (destinationSetter.target != player.transform)
		{
			FlipDirectionReversed();
		}
		else
		{
			FlipDirection();
		}

		if (damageTimer > 0) {
			damageTimer -= Time.deltaTime;
		}

		if (destinationSetter.target == null) {
			destinationSetter.target = player.transform;
			aipath.maxSpeed = aipathMaxSpeed;
			gameObject.layer = 9;
		}
	}

    private void startEnemy()
    {
        isStopped = false;
        mainSprite.SetActive(true);
        alternateSprite.SetActive(false);
        aipath.canMove = true;
        myRigidbody.gravityScale = 1;
        myRigidbody.bodyType = RigidbodyType2D.Dynamic;

        var guo = new GraphUpdateObject(capsuleCollider.bounds);
        guo.modifyWalkability = true;
        guo.setWalkability = true;
        AstarPath.active.UpdateGraphs(guo);
    }

    private void StopEnemy()
    {
        isStopped = true;
        mainSprite.SetActive(false);
        alternateSprite.SetActive(true);
        aipath.canMove = false;
        myRigidbody.gravityScale = 0;
        myRigidbody.velocity = new Vector2(0, 0);
        myRigidbody.bodyType = RigidbodyType2D.Static;

        var guo = new GraphUpdateObject(capsuleCollider.bounds);
        guo.modifyWalkability = true;
        guo.setWalkability = false;
        AstarPath.active.UpdateGraphs(guo);
    }

    public void reduceHealth(float damage) {
		health -= damage;
		if (health <= 0 && !isDead) {
			isDead = true;
			Destroy(gameObject);
			SoundManager.instance.PlayOneShot(deathClip);
			GameController.instance.EnemiesKilled++;
			GameController.instance.AddEnemyType(maskTypeToActivate);

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

    private bool TryDamagePlayer()
    {
		if (damageTimer > 0)
		{
			return false;
		}

        DamagePlayer();
        return true;
    }


    private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			TryDamagePlayer();
		}
	}
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill"))
        {
            SkillConfig SkillConfig = collision.GetComponentInParent<SkillConfig>();
            if (SkillConfig.MaskType == maskTypeToActivate)
            {
                reduceHealth(SkillConfig.GetDamage());
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
			TryDamagePlayer();
        }
    }

	private void OnParticleCollision(GameObject particle)
	{
		SkillConfig particleParent = particle.GetComponentInParent<SkillConfig>();
		if (particleParent.MaskType == maskTypeToActivate) {
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
