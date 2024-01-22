using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float throwForce = 10f;
    public Dodgeball dodgeballPrefab;
    public GameObject playerFloor;
    private Vector3 moveInput;
    private PlayerControls controls;
    private Dodgeball currentDodgeball;
    private Vector3 throwDirection;
    public Slider powerBar;

    private bool canPickBall = true;
    private float pickBallDelay = 1.0f;
    private bool isCharging = false;



    private void Awake()
    {
        controls = new PlayerControls();

    controls.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector3>();
        controls.Player.Throw.started += ctx => StartCharging();
        controls.Player.Throw.canceled += ctx => ThrowBall();
      //  controls.Player.OpenMenu.performed += ctx => OpenMenu();

        transform.position = new Vector3(transform.position.x, 5, transform.position.z);
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

        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.z);
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;

        // Get the corners of the floor object so the player stays within them
        Bounds floorBounds = playerFloor.GetComponent<BoxCollider>().bounds;

        // Use Clamp to ensure that the x and z positions stay within the corners
        newPosition.x = Mathf.Clamp(newPosition.x, floorBounds.min.x, floorBounds.max.x);
        newPosition.z = Mathf.Clamp(newPosition.z, floorBounds.min.z, floorBounds.max.z);


        transform.position = newPosition;

        if (isCharging)
        {
            // Nostetaan throwforcea ja clamp hoitaa sit 10-20 välillä forcea riippuu kauanko painetaan
            throwForce = Mathf.Clamp(throwForce + Time.deltaTime * 50, 50, 100);

            float normalizedThrowForce = (throwForce - 50) / 50;
            powerBar.value = throwForce;
            //Sliderin value on vaan yksi sain sen näin täyttymään suht kivasti ku jako floatiksi
            powerBar.value = normalizedThrowForce;
            powerBar.gameObject.SetActive(true);
        }
    }


    void OnCollisionEnter(Collision collision)
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
            currentDodgeball.Throw(new Vector3(0, 0.2f, -1), throwForce, gameObject);
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