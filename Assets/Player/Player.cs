using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject gameFloor;
    [SerializeField] private Slider powerBar;
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] private GameObject outOfGame;
    [SerializeField] private PlayerControls controls;
    [SerializeField] private Vector3 startingPosition;
    public Dodgeball dodgeballPrefab;
    private Dodgeball currentDodgeball;
    private Vector3 moveInput;
    private Vector3 throwDirection;
    public float throwForce = 10f;
    public float moveSpeed = 10.0f;

    private bool canPickBall = true;
    private float pickBallDelay = 1.0f;
    private bool isCharging = false;



    private void Awake()
    {
        Init();

        controls.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector3>();
        controls.Player.Throw.started += ctx => StartCharging();
        controls.Player.Throw.canceled += ctx => ThrowBall();
      //  controls.Player.OpenMenu.performed += ctx => OpenMenu();
    }

    private void Init()
    {
        controls = new PlayerControls();
        playerRigidbody = GetComponent<Rigidbody>();
        gameFloor = GameObject.Find("Floor");
        outOfGame = GameObject.Find("PlayerOutOfGamePos");
        transform.position = startingPosition;
        // gameManager = FindObjectOfType<GameManager>();
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
        Bounds floorBounds = gameFloor.GetComponent<BoxCollider>().bounds;

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

    public void PickBall(Dodgeball dodgeball)
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

    public void Die()
    {
        transform.position = outOfGame.transform.position;
        moveSpeed = 0;
        playerRigidbody.useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        GameManager.Instance.RestartGame();
    }
}