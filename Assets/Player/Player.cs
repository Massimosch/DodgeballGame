using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject gameFloor;
    [SerializeField] private Slider powerBar;
    [SerializeField] private Slider dodgeSlider;
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] private GameObject outOfGame;
    [SerializeField] private Vector3 startingPosition;
    [SerializeField] private MiddleLine middleLine;
    // private Animator animator;
    private PlayerControls controls;
    private GameManager gameManager;
    public Dodgeball dodgeballPrefab;
    private Dodgeball currentDodgeball;
    private Vector3 moveInput;
    private Vector3 throwDirection;
    public Transform ballHolder;
    private bool canPickBall = true;
    private bool isCharging = false;
    private bool HaveDodgeball = false;
    private bool isUpdatingSlider = false;
    public float throwForce = 10f;
    public float moveSpeed = 10.0f;
    private float pickBallDelay = 1.0f;
    private float DodgeChanceValue = 0f;
    private float MaxDodgeSliderValue = 1f;
    private float DodgeSliderIncreaseAmount = 0.015f;




    private void Awake()
    {
        Init();

        controls.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector3>();
        controls.Player.Throw.started += ctx => StartCharging();
        controls.Player.Throw.canceled += ctx => ThrowBall();
        controls.Player.Dodge.performed += ctx => Dodge();
        StartCoroutine(IncreaseDodgeChanceOverTime());
        //controls.Player.OpenMenu.performed += ctx => OpenMenu();
    }


    private void Init()
    {
        //animator = GetComponent<Animator>();
        controls = new PlayerControls();
        playerRigidbody = GetComponent<Rigidbody>();
        gameFloor = GameObject.Find("Floor");
        outOfGame = GameObject.Find("PlayerOutOfGamePos");
        transform.position = startingPosition;
        gameManager = FindObjectOfType<GameManager>();
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
        HandleMovement();
        HandleCharging();
    }


    void OnCollisionEnter(Collision collision)
    {
        if (canPickBall && collision.gameObject.CompareTag("Dodgeball") && currentDodgeball == null)
        {
            PickBall(collision.gameObject.GetComponent<Dodgeball>());
        }
    }

    void HandleMovement()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.z);
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;

        // Get the corners of the floor object so the player stays within them
        Bounds floorBounds = gameFloor.GetComponent<BoxCollider>().bounds;

        // Use Clamp to ensure that the x and z positions stay within the corners
        newPosition.x = Mathf.Clamp(newPosition.x, floorBounds.min.x, floorBounds.max.x);
        newPosition.z = Mathf.Clamp(newPosition.z, floorBounds.min.z, floorBounds.max.z);

        transform.position = newPosition;

        //animator.SetFloat("MoveX", moveInput.x);
        //animator.SetFloat("MoveY", moveInput.z);
    }

    public void PickBall(Dodgeball dodgeball)
    {
        HaveDodgeball = true;
        currentDodgeball = dodgeball;
        Rigidbody dodgeballRb = dodgeball.GetComponent<Rigidbody>();
        dodgeballRb.isKinematic = true;
        currentDodgeball.transform.SetParent(ballHolder);
        currentDodgeball.transform.position = ballHolder.position;
        throwForce = 10;
    }

    public bool HasDodgeball
    {
        get { return HaveDodgeball; }
    }

    void StartCharging()
    {
        if (!middleLine.isLineDeadly)
        {
            Debug.Log("Cannot start charging because the ball is not activated at PlayerLine.");
            return;
        }

        if (currentDodgeball != null)
        {
            isCharging = true;
        }
    }

    void HandleCharging()
    {
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


    public void ThrowBall()
    {
    
    
    if (!middleLine.isLineDeadly)
        {
            Debug.Log("Cannot throw because the ball is not activated at PlayerLine.");
            return;
        }

    if (currentDodgeball != null)
        {
            currentDodgeball.GetComponent<Rigidbody>().isKinematic = false;
            currentDodgeball.transform.SetParent(null);
            currentDodgeball.Throw(new Vector3(0, 0.2f, -1), throwForce, gameObject);
            currentDodgeball = null;

            HaveDodgeball = false;
            canPickBall = false;
            StartCoroutine(EnablePickBallAfterDelay());

            powerBar.gameObject.SetActive(false);
            isCharging = false;
        }
    }

    private IEnumerator EnablePickBallAfterDelay()
    {
    yield return new WaitForSeconds(pickBallDelay);
    canPickBall = true;
    }
 
    void OnDodgeChanceValueChanged(float newDodgeChanceValue)
    {
        if (!isUpdatingSlider && newDodgeChanceValue != DodgeChanceValue)
        {
            StopIncreasingDodgeChance();
            DodgeChanceValue = newDodgeChanceValue;
            if (DodgeChanceValue < MaxDodgeSliderValue)
            {
                StartIncreasingDodgeChance();
            }
        }
    }

    private IEnumerator IncreaseDodgeChanceOverTime()
    {
        float increaseAmount = DodgeSliderIncreaseAmount;
        float returnTime = 0.25f;
        float overFiftyAmount = 0.005f;

        while (DodgeChanceValue < MaxDodgeSliderValue)
        {
            if (DodgeChanceValue >= 0.5f)
            {
                increaseAmount = overFiftyAmount;
            }

            DodgeChanceValue += increaseAmount;
            isUpdatingSlider = true;
            dodgeSlider.value = DodgeChanceValue;
            isUpdatingSlider = false;
            yield return new WaitForSeconds(returnTime);
        }
    }

    void Dodge()
        {
            float dodgePhase = 1f;
            float randomNumber = UnityEngine.Random.value;
            Debug.Log("Dodgen Tapahtumiseen tarvitaan: " + randomNumber);
            Debug.Log("Pelaajalla on " + DodgeChanceValue);

                // jos randomnumber on vähemmän tai yht suurikuin dodgechance, dodgetaan
                if (randomNumber <= DodgeChanceValue)
                {
                    Debug.Log("🎉🎉 DODGE ONNISTUI! 🏋️‍♂️🦤");
                    GetComponent<BoxCollider>().enabled = false;
                    playerRigidbody.useGravity = false;
                    StartCoroutine(TurnOnColliderAfterDelay(dodgePhase)); // dodgen kesto aika?
                }
            // Resetoidaan dodge himmelit
            DodgeChanceValue = 0f;
            isUpdatingSlider = true;
            dodgeSlider.value = DodgeChanceValue;
            isUpdatingSlider = false;

            StopIncreasingDodgeChance();

            // aloita Dodgen Sliderin valuen increase kun dodgetila ohi
            StartCoroutine(StartIncreasingDodgeChanceAfterDelay(dodgePhase));
        }

    private IEnumerator StartIncreasingDodgeChanceAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartIncreasingDodgeChance();
        }
    private void StartIncreasingDodgeChance()
        {
            StartCoroutine(IncreaseDodgeChanceOverTime());
        }

    private void StopIncreasingDodgeChance()
        {
            StopCoroutine(IncreaseDodgeChanceOverTime());
        }


        private IEnumerator TurnOnColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<BoxCollider>().enabled = true;
        playerRigidbody.useGravity = true;
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