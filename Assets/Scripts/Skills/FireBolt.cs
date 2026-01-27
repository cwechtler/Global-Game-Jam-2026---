using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : SkillConfig
{
	[SerializeField] private float range = 1f;
	[Space]
	[Tooltip("The particle system to play.")]
	[SerializeField] private ParticleSystem ProjectileParticleSystem;
	[Tooltip("The particle system to play upon collision.")]
	[SerializeField] private ParticleSystem ProjectileExplosionParticleSystem;
	[Range(.01f, 10f)] [SerializeField] private float areaEffect = 6f;

	[Tooltip("The sound to play upon collision.")]
	[SerializeField] private AudioSource ProjectileCollisionSound;

	private bool stop = false;
	ParticleSystem.MainModule main;

	void Start()
	{
		transform.up = GetComponent<Rigidbody2D>().velocity;
		if (ProjectileParticleSystem) {
			ProjectileParticleSystem.Play();
		}
		main = ProjectileParticleSystem.main;
		StartCoroutine(DestroySkill(range));
	}

	void Update()
	{
		if (ProjectileParticleSystem && !ProjectileParticleSystem.isPlaying) {
			Destroy(gameObject);
		}

		if (stop && ProjectileParticleSystem != null) {
			Color col = new Color(255,255,255,255);
			col.a -= col.a * 2f * Time.deltaTime;
			main.startColor = col;
		}
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obsticle") || collision.gameObject.CompareTag("Destructible")) {
			this.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
			var colls = Physics2D.OverlapCircleAll(transform.position, areaEffect);
			foreach (var col in colls) {
				if (col.CompareTag("Enemy")) {
					Enemy enemy = col.GetComponent<Enemy>();

					if (enemy.SkillElementTypeToDestroy == skillElementType) {
						enemy.reduceHealth(GetDamage());
					}
				}
			}

			if (ProjectileParticleSystem != null) {
				Destroy(ProjectileParticleSystem.gameObject);
			}

			if (ProjectileExplosionParticleSystem != null) {
				ProjectileExplosionParticleSystem.Play();
			}

			if (ProjectileCollisionSound != null) {
				ProjectileCollisionSound.Play();
			}

			Destroy(gameObject, ProjectileCollisionSound.clip.length);
		}
	}

	private IEnumerator DestroySkill(float skillDuration)
	{
		yield return new WaitForSeconds(skillDuration);
		stop = true;
		yield return new WaitForSeconds(1.5f);
		gameObject.GetComponent<CircleCollider2D>().enabled = false;
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, areaEffect);
	}
}
