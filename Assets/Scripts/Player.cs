using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public Dodgeball dodgeballScript;

    private Vector2 moveInput;
    private PlayerControls controls;
    private bool isHoldingBall = false;

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
        transform.position += movement * moveSpeed * Time.deltaTime;
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

    void ThrowBall()
    {
        if (isHoldingBall)
        {
            dodgeballScript.Throw(new Vector2(moveInput.x, moveInput.y));
            isHoldingBall = false;
        }
    }
}