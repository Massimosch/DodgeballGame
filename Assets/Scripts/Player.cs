using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float throwForce = 10.0f;

    public float catchRadius = 2.0f;

    public Dodgeball dodgeballScript;
    public GameObject playerFloor;

    private Vector2 moveInput;
    private PlayerControls controls;
    private bool isHoldingBall = false;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Throw.performed += ctx => ThrowBall();
        controls.Player.Catch.performed += ctx => CatchBall();

    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0);
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;


                // Get the bounds of playerFloor
        Bounds floorBounds = playerFloor.GetComponent<BoxCollider2D>().bounds;

        // Check if the new position is within the bounds
        newPosition.x = Mathf.Clamp(newPosition.x, floorBounds.min.x, floorBounds.max.x);
        newPosition.y = Mathf.Clamp(newPosition.y, floorBounds.min.y, floorBounds.max.y);

        // Apply the adjusted position
        transform.position = newPosition;

        CheckForThrownBalls();
    }

void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Dodgeball") && !isHoldingBall)
    {
        dodgeballScript = other.gameObject.GetComponent<Dodgeball>();
        isHoldingBall = true;
        dodgeballScript.PickUp(transform);
    }
}


    private void ThrowBall()
    {
        if (dodgeballScript != null)
        {
            Vector2 direction = transform.right;

            dodgeballScript.Throw(direction, throwForce);
            isHoldingBall = false;
        }
    }
    private void CatchBall()
    {
        if (!isHoldingBall)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, catchRadius);
            foreach (Collider2D collider in colliders)
            {
                Dodgeball dodgeball = collider.GetComponent<Dodgeball>();
                if (dodgeball != null && !dodgeball.isOnFloor)
                {
                    dodgeballScript = dodgeball;
                    dodgeballScript.PickUp(transform);
                    isHoldingBall = true;
                    break;
                }
            }
        }
    }

private void CheckForThrownBalls()
{
    if (!isHoldingBall)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, catchRadius);
        foreach (Collider2D collider in colliders)
        {
            Dodgeball dodgeball = collider.GetComponent<Dodgeball>();
            if (dodgeball != null && !dodgeball.isOnFloor && dodgeball.ThrownBy != gameObject)
            {
                Die();
                break;
            }
        }
    }
}

    private void Die()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
{
    // Draw a yellow sphere at the player's position with a radius of catchRadius
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, catchRadius);
}
}