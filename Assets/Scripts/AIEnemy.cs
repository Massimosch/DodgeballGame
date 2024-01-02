using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public Dodgeball dodgeballScript;
    public float throwForce = 5f;
    public GameObject ball;
    public GameObject player;
    public float pickRadius = 1f;

    private Vector3 moveDirection;
    private float moveTimer = 1f;
    public GameObject enemyFloor;

    void Start()
    {
        dodgeballScript = ball.GetComponent<Dodgeball>();
        StartCoroutine(StartBehavior());
    }

IEnumerator StartBehavior()
{
    yield return new WaitForSeconds(4);

    // Get the bounds of the enemy floor
    var floorBounds = enemyFloor.GetComponent<BoxCollider2D>().bounds;

    // Store the initial Y value
    float initialY = transform.position.y;

    // Move to the end of the negative x value of the enemy floor bounds
    Vector3 leftEnd = new Vector3(floorBounds.min.x, initialY, transform.position.z);
    while (Vector3.Distance(transform.position, leftEnd) > pickRadius)
    {
        moveDirection = (leftEnd - transform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        yield return null;
    }

    // Move back to the middle of the enemy floor
    Vector3 middle = new Vector3(floorBounds.center.x, initialY, transform.position.z);
    while (Vector3.Distance(transform.position, middle) > pickRadius)
    {
        moveDirection = (middle - transform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        yield return null;
    }

    // Start random movement
    StartCoroutine(RandomMovement());
}

IEnumerator RandomMovement()
{
    while (true)
    {
        // Move the AI enemy
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Change direction every second
        moveTimer += Time.deltaTime;
        if (moveTimer > 1)
        {
            // Get the bounds of enemyFloor
            Bounds floorBounds = enemyFloor.GetComponent<BoxCollider2D>().bounds;

            // Generate a random point in enemyFloor object to move around
            Vector3 randomPoint = new Vector3(
                Random.Range(floorBounds.min.x, floorBounds.max.x),
                Random.Range(floorBounds.min.y, floorBounds.max.y),
                transform.position.z
            );

            // Calculate direction to move
            moveDirection = (randomPoint - transform.position).normalized;
            moveTimer = 0;
        }

        // Check if ball is within pickRadius
        if (Vector3.Distance(transform.position, ball.transform.position) <= pickRadius)
        {
            // Pick up ball
            dodgeballScript.PickUp(transform);

            // Hold ball for 1-4 seconds
            yield return new WaitForSeconds(Random.Range(1, 5));

            // Throw ball to player
            dodgeballScript.Throw(player.transform.position - transform.position, Random.Range(5, 15));
        }

        yield return null;
    }
}
}
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AIEnemy : MonoBehaviour
// {
//     public float moveSpeed = 10.0f;
//     public Dodgeball dodgeballScript;
//     public float throwForce = 5f;
//     public GameObject ball;
//     public GameObject player;
//     public float pickRadius = 0.3f;

//     private Vector3 moveDirection;
//     private float moveTimer = 0;
//     public GameObject enemyFloor;

//     void Update()
//     {
//         // Move the AI enemy
//         transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

//         // Change direction every second
//         moveTimer += Time.deltaTime;
//         if (moveTimer > 1)
//         {
//             // Get the bounds of enemyFloor
//             Bounds floorBounds = enemyFloor.GetComponent<BoxCollider2D>().bounds;

//             // Generate a random point in enemyFloor object to move around
//             Vector3 randomPoint = new Vector3(
//                 Random.Range(floorBounds.min.x, floorBounds.max.x),
//                 Random.Range(floorBounds.min.y, floorBounds.max.y),
//                 transform.position.z
//             );

//             // Calculate direction to move
//             moveDirection = (randomPoint - transform.position).normalized;
//             moveTimer = 0;
//         }

// // Pick up the ball
// Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickRadius);
// foreach (Collider2D collider in colliders)
// {
//     Dodgeball dodgeball = collider.GetComponent<Dodgeball>();
//     if (dodgeball != null && dodgeball.isOnFloor)
//     {
//         dodgeballScript = dodgeball;
//         dodgeballScript.PickUp(transform);
//         break;
//     }
// }

//         // Throw the ball
//         if (dodgeballScript != null && dodgeballScript.transform.parent == transform)
//         {
//             Vector2 direction = (player.transform.position - transform.position).normalized;
//             dodgeballScript.Throw(direction, throwForce);
//         }
//     }
// }