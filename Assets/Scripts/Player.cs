using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float throwForce = 10f;
    public Dodgeball dodgeball;
    public GameObject playerFloor;
    private Vector2 moveInput;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Throw.performed += ctx => ThrowBall();
    //    controls.Player.Catch.performed += ctx => CatchBall();

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

        transform.position = newPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Dodgeball"))
    {
        dodgeball = other.gameObject.GetComponent<Dodgeball>();
    }
}

    public void ThrowBall()
    {
    Vector2 direction = new Vector2(1, 0); // Positive x direction
    dodgeball.Throw(direction, throwForce);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}