using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private float damage = 10f;
    private AudioClip orbCollisionSound;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 2f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obsticle") || collision.gameObject.CompareTag("Destructible"))
        {
            //this.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            //var colls = Physics2D.OverlapCircleAll(transform.position, AreaEffect);
            //Debug.Log("Projectile hit something, colliders found: " + colls.Length);
            //if (hasEndAnimation)
            //    animator.SetBool("HitSomething", true);
            //foreach (var col in colls)
            //{
            //    Debug.Log("Collider found: " + col.name + " with tag: " + col.tag);
                if (collision.CompareTag("Enemy"))
                {
                    Debug.Log("Applying damage to enemy: " + collision.name);
                    Enemy enemy = collision.GetComponentInParent<Enemy>();
                    enemy.reduceHealth(damage);
                }
            //}
        }
    }
}
