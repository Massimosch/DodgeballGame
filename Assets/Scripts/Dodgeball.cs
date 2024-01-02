using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgeball : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isHeld = false;
    public bool isOnFloor = false;
    
    public GameObject ThrownBy { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Create a new PhysicsMaterial2D
        PhysicsMaterial2D bouncyMaterial = new PhysicsMaterial2D();
        bouncyMaterial.bounciness = 1;
        bouncyMaterial.friction = 0;

        // Get the CircleCollider2D component of the dodgeball
        CircleCollider2D dodgeballCollider = GetComponent<CircleCollider2D>();

        // Set the material of the dodgeball's collider
        dodgeballCollider.sharedMaterial = bouncyMaterial;
    }


void Update()
{
    // Check if the dodgeball is not being held and has stopped moving
    if (!isHeld && rb.velocity.magnitude < 0.01f)
    {
        isOnFloor = true;
    }
    else
    {
        isOnFloor = false;
    }
}

    public void PickUp(Transform playerTransform)
    {
        transform.SetParent(playerTransform);
        rb.velocity = Vector2.zero;
        isHeld = true;
        ThrownBy = playerTransform.gameObject;
    }
    public void Throw(Vector2 direction, float force)
    {
        // Remove the dodgeball from the player's hierarchy
        transform.parent = null;

        // Add force to the dodgeball in the given direction
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        // Set isHeld to false to indicate that the dodgeball is no longer being held
        isHeld = false;
        StartCoroutine(ClearThrownBy());
    }
        private IEnumerator ClearThrownBy()
    {
        yield return new WaitForSeconds(0.5f);
        ThrownBy = null;
    }

void OnCollisionEnter2D(Collision2D collision)
{
    // Check if the ball collides with an enemy
    if (collision.gameObject.CompareTag("Enemy") && ThrownBy != null)
    {
        // Get the AIEnemy component
        AIEnemy enemy = collision.gameObject.GetComponent<AIEnemy>();
        
        // Check if the ball was thrown by the player
        Player player = ThrownBy.GetComponent<Player>();
        if (player != null && enemy != null)
        {
            // The enemy dies
            Destroy(collision.gameObject);
        }
    }
    // Reduce the ball's velocity
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    rb.velocity *= 0.5f; // Adjust this value to control how much the ball slows down

    // Add a force in the opposite direction of the collision
    Vector2 force = collision.contacts[0].normal * 100f; // Adjust this value to control the strength of the force
    rb.AddForce(force);
}
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     Debug.Log("Dodgeball collided with: " + collision.gameObject.name);
    //     // check if the dodgeball is not being held (has been thrown)
    //     if (!isHeld)
    //     {
    //         // check if the dodgeball has collided with an enemy after throw
    //         if (collision.gameObject.CompareTag("Enemy"))
    //         {
    //             Destroy(collision.gameObject);
    //             rb.velocity = Vector2.zero;
    //         }
    //         // check if the dodgeball has collided with a wall
    //         else if (collision.gameObject.CompareTag("Wall"))
    //         {
    //             // No need to manually reverse the direction of the dodgeball
    //             // Unity's physics engine will handle the bouncing
    //         }
    //     }
    // }
}