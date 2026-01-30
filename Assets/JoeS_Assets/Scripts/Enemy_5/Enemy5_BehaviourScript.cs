using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5_BehaviourScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public float speed = .2f; // Adjust speed in the Inspector
    public Vector2 minBounds;
    public Vector2 maxBounds;
    private Vector3 targetPosition;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    private Vector2 currentMovementDirection;
    void Start()
    {
         rb = GetComponent<Rigidbody2D>();
         SetNewTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsRunning", true);
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the object has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
        
    }
    void FixedUpdate() // Use FixedUpdate for Rigidbody operations
    {
        // Get input
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        // Set the Rigidbody's velocity
        //rb.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);
        //rb.velocity = currentMovementDirection * speed;

    }
   void SetNewTargetPosition()
    {
        // Choose a random position within the defined bounds
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);
        targetPosition = new Vector3(randomX, randomY, transform.position.z);
    }
}

