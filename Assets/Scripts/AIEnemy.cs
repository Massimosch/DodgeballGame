using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float throwForce = 20.0f;
    public GameObject ball;
    public GameObject player;

    private Vector3 moveDirection;
    private float moveTimer = 0;
    public GameObject enemyFloor;

void Update()
{
    // moving in the decided direction....
    transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

    // changing direction every second?? maybe too much
    moveTimer += Time.deltaTime;
    if (moveTimer > 1)
    {
        // getting the bounds of enemyFloor
        Bounds floorBounds = enemyFloor.GetComponent<BoxCollider2D>().bounds;

        // generating random points in enemyFloor object to move around
Vector3 randomPoint = new Vector3(
    Random.Range(floorBounds.min.x, floorBounds.max.x),
    Random.Range(floorBounds.min.y, floorBounds.max.y),
    transform.position.z
);
        // calculating direction to move
        moveDirection = (randomPoint - transform.position).normalized;
        moveTimer = 0;
    }

    // If the AI has the ball?? Throw Player??
    // ...

    // Implementing dodging....? When player throw ball some chance for sucessful dodge?
    // ...
}
}