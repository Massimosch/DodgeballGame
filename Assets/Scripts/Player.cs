using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float throwForce = 10f;
    public Dodgeball dodgeballPrefab;
    public GameObject playerFloor;
    private Vector2 moveInput;
    private PlayerControls controls;
    private Dodgeball currentDodgeball;
    private Vector2 throwDirection = Vector2.zero;


    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Throw.performed += ctx => ThrowBall();
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Dodgeball") && currentDodgeball == null)
        {
            currentDodgeball = collision.gameObject.GetComponent<Dodgeball>();
            currentDodgeball.transform.SetParent(transform);
        }
    }

    public void ThrowBall()
    {
    if (currentDodgeball != null)
        {
            currentDodgeball.transform.SetParent(null);
            Vector2 throwDirection = new Vector2(0, 1);
            currentDodgeball.Throw(throwDirection, throwForce, gameObject); // Tässä lisätty 'gameObject'
            currentDodgeball = null;
        }
    }


    void Die()
    {
        Destroy(gameObject);
    }
}