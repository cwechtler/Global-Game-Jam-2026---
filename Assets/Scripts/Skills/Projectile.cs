using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float areaEffect = 3f;
    private float damage = 10f;
    private AudioClip projectileCollisionSound;

    public float AreaEffect { get => areaEffect; set => areaEffect = value; }
    public float Damage { get => damage; set => damage = value; }
    public AudioClip ProjectileCollisionSound { get => projectileCollisionSound; set => projectileCollisionSound = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obsticle") || collision.gameObject.CompareTag("Destructible"))
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            var colls = Physics2D.OverlapCircleAll(transform.position, AreaEffect);
            Debug.Log("Projectile hit something, colliders found: " + colls.Length);
            // animator.setbool("HitSomething");
            foreach (var col in colls)
            {
                Debug.Log("Collider found: " + col.name + " with tag: " + col.tag);
                if (col.CompareTag("Enemy"))
                {
                    Debug.Log("Applying damage to enemy: " + col.name);
                    Enemy enemy = col.GetComponentInParent<Enemy>();
                    enemy.reduceHealth(damage);              
                }
            }
            Destroy(gameObject);

            //Destroy(gameObject, ProjectileCollisionSound.length); // Delay destruction to allow animation to finish.
        }
    }

    private IEnumerator DestroySkill(float skillDuration)
    {
        yield return new WaitForSeconds(skillDuration);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AreaEffect);
    }
}
