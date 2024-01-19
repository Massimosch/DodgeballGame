using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public Slider powerBar;

    private bool canPickBall = true;
    private float pickBallDelay = 1.0f;
    private bool isCharging = false;



    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Throw.started += ctx => StartCharging();
        controls.Player.Throw.canceled += ctx => ThrowBall();
      //  controls.Player.OpenMenu.performed += ctx => OpenMenu();
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

        // Haetaan floor objectin kulmat jotta pelaaja pysyy niiden sisässä
        Bounds floorBounds = playerFloor.GetComponent<BoxCollider2D>().bounds;

        // Checkkailaan Clampillä että x ja y positiot pysyy kulmien sisällä
        newPosition.x = Mathf.Clamp(newPosition.x, floorBounds.min.x, floorBounds.max.x);
        newPosition.y = Mathf.Clamp(newPosition.y, floorBounds.min.y, floorBounds.max.y);

        transform.position = newPosition;

        if (isCharging)
        {
            // Nostetaan throwforcea ja clamp hoitaa sit 10-20 välillä forcea riippuu kauanko painetaan
            throwForce = Mathf.Clamp(throwForce + Time.deltaTime * 10, 10, 20);

            float normalizedThrowForce = (throwForce - 10) / 10;
            powerBar.value = throwForce;
            //Sliderin value on vaan yksi sain sen näin täyttymään suht kivasti ku jako floatiksi
            powerBar.value = normalizedThrowForce;
            powerBar.gameObject.SetActive(true);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
    if (canPickBall && collision.gameObject.CompareTag("Dodgeball") && currentDodgeball == null)
    {
        PickBall(collision.gameObject.GetComponent<Dodgeball>());
    }
    }

    private void PickBall(Dodgeball dodgeball)
    {
        currentDodgeball = dodgeball;
        currentDodgeball.transform.SetParent(transform);
        throwForce = 10;
    }

    void StartCharging()
    {
        if (currentDodgeball != null)
        {
            isCharging = true;
        }
    }


    public void ThrowBall()
    {
        if (currentDodgeball != null)
        {
            currentDodgeball.transform.SetParent(null);
            Vector2 throwDirection = new Vector2(1, 0);
            currentDodgeball.Throw(throwDirection, throwForce, gameObject);
            currentDodgeball = null;

            canPickBall = false;
            StartCoroutine(EnablePickBallAfterDelay());

            powerBar.gameObject.SetActive(false);

            isCharging = false;
        }
    }

   // void OpenMenu()

    private IEnumerator EnablePickBallAfterDelay()
    {
    yield return new WaitForSeconds(pickBallDelay);
    canPickBall = true;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}