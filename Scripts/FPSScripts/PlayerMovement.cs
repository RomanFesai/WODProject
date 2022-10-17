using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //public GameObject bullet;
    //public GameObject ShootHole;
    //public float bulletSpeed = 100f;
    public string labelText = "Survive!";
    private GUIStyle guiStyle = new GUIStyle();
    //Assingables
    public Transform playerCam;
    public Transform orientation;
    public Transform playerHand;
    public bool gunSwayReady = false;
    public PickUpThrow isCarry;
    [SerializeField] private GameBehaviour _gameManager;
    [SerializeField] private GameObject AmmoInformation;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private CanvasGroup DamageInformation;
    /*[Header("Player Step Climb")]
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 0.1f;*/

    //Other
    private Rigidbody rb;
    bool timeToggle;
    float defaultTimeScale;
    float defaultFixedDeltaTime;
    public float timeScale;

    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;
    private bool isCrouching = false;
    public static bool isSprinting = false;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    //Jumping
    private bool readyToJump = true;
    [SerializeField] private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    //Input
    float x, y;
    bool jumping, sprinting, crouching;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    //Picking up weapons
    public Transform ObjectHolder;
    public float ThrowForce;
    public bool carryObject = false;
    private GameObject Item;
    private GameObject Item2;
    public bool IsThrowable;
    public Vector3 posVel;
    //��������� �������, ���� �������� ��� ���������� ����������� 
    public GunSway GunSwayPistolEnabled;
    public GunSway GunSwayAntiGravGunEnabled;

    public GameObject E_Key;

    [Header("FootStep Parametrs")]
    [SerializeField] private float baseStepSpeed = 0.2f;
    [SerializeField] private float crouchStepMultipler = 1.5f;
    [SerializeField] private float sprintStepMultipler = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] MudStepClips = default;
    [SerializeField] private AudioClip[] WaterStepClips = default;
    [SerializeField] private AudioClip[] MetalStepClips = default;
    public float footStepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultipler : isSprinting ? baseStepSpeed * sprintStepMultipler : baseStepSpeed;
    public static bool isWalking = false;

    public Animator transition;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
    }

    void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //_gameManager = GameObject.Find("GameBehaviour").GetComponent<GameBehaviour>();
        defaultTimeScale = 1.0f;
        defaultFixedDeltaTime = 0.02f;

        if (Utilities.playerDeaths <= 0)
        { StartCrouch(); }
        else { StopCrouch(); }
        //E_Key.SetActive(false);
        FindObjectOfType<AudioManager>().Stop("WellTheme");
    }


    private void FixedUpdate()
    {
        Movement();
        //StepClimb();
    }

    private void Update()
    {
        MyInput();
        Look();
        checkWalk();
        //SlowMotion();
        RayCastGuns();
        if (isWalking)
        { 
            Handle_FootSteps();
        }
        Debug.DrawRay(playerCam.transform.position, Vector3.up);
        Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward);
    }

    public void checkWalk()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            isWalking = true;
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)))
        {
            isWalking = false;
        }
    }

    private void RayCastGuns()
    {
        RaycastHit hit;
        Ray Vision = new Ray(playerCam.transform.position, playerCam.transform.forward);

        RayCastShotgun gun2P = GameObject.Find("shotgun").GetComponent<RayCastShotgun>();
        //��������� ������� �������� � GunSway'��
        GunSwayPistolEnabled = GameObject.Find("GunSw").GetComponent<GunSway>();
        //GameObject GunSlot = GameObject.Find("hand");
        if (Physics.Raycast(Vision, out hit, 2))
        {
            String ObjectTag = hit.transform.tag;
            switch (ObjectTag)
            {
                case "Shotgun":
                    EKeyHintActive();

                    if (Input.GetKeyDown(KeyCode.E) && playerHand.childCount < 1)
                    {
                        //Destroy(GunSlot.transform.GetChild(0).gameObject);
                        Item = hit.collider.gameObject;
                        //Item.transform.parent = playerHand.gameObject.transform;
                        Item.transform.SetParent(playerHand);
                        Item.transform.localPosition = Vector3.zero;
                        Item.transform.localRotation = Quaternion.Euler(Vector3.zero);
                        foreach (Transform transform in Item.GetComponentsInChildren<Transform>(true))
                        { transform.gameObject.layer = 8; }
                        //Item.transform.localPosition = Vector3.SmoothDamp(Item.transform.localPosition, Vector3.zero, ref posVel, 1/20);
                        //Item.transform.localRotation = Quaternion.Slerp(Item.transform.localRotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * 2); 
                        gun2P.gunready1 = true;
                        AmmoInformation.SetActive(true);
                        healthBar.SetActive(true);
                        //gun1A.gunready = false;
                        //GunSwayAntiGravGunEnabled.enabled = false;
                        GunSwayPistolEnabled.enabled = true;
                        gunSwayReady = true;
                        //Item.GetComponent<Rigidbody>().useGravity = false;
                        //Item.GetComponent<Rigidbody>().isKinematic = true; //"disabling" the rigidbody (it's still active but gravity won't apply to it.
                        Item.GetComponent<BoxCollider>().enabled = false; //disabling the collider.
                        EKeyHintDisabled();
                        FindObjectOfType<AudioManager>().Play("PickUp");
                        Debug.Log("Did Hit");
                    }
                    break;
                case "Ammo":
                    EKeyHintActive();
                    if (Input.GetKeyDown(KeyCode.E) && gun2P.gunready1 == true)
                    {
                        Item = hit.collider.gameObject;
                        Destroy(Item);
                        FindObjectOfType<AudioManager>().Play("PickUp");
                        RayCastShotgun.magazineSize += 5;
                        EKeyHintDisabled();
                    }
                    break;
                case "Exit":
                    EKeyHintActive();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(LoadLevel("fps_ending"));
                        EKeyHintDisabled();
                    }
                    break;
                case "TakeObject":
                    EKeyHintActive();
                    break;
                default:
                    EKeyHintDisabled();
                    break;
            }
        }
        else
        {
            EKeyHintDisabled();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isCarry.carrying == true)
            {
                isCarry.dropObject();
                isCarry.carrying = false;
            }
            /*else if (isCarry.carrying == false && playerHand.childCount >= 1)
            {
                playerHand.DetachChildren();
                Item.GetComponent<Rigidbody>().isKinematic = false;
                Item.GetComponent<Rigidbody>().useGravity = true;
                Item.GetComponent<BoxCollider>().enabled = true;
                Item.GetComponent<Rigidbody>().AddForce(Item.transform.forward * ThrowForce);
                foreach (Transform transform in Item.GetComponentsInChildren<Transform>(true))
                { transform.gameObject.layer = 0; }
                gun2P.gunready1 = false;
                //gun1A.gunready = false;
                gunSwayReady = false;
            }*/
        }
    }

    public void EKeyHintActive()
    {
        E_Key.SetActive(true);
    }

    public void EKeyHintDisabled()
    {
        E_Key.SetActive(false);
    }

    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouching && !Physics.Raycast(playerCam.transform.position, Vector3.up, 1f))
        { 
            StartCrouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching && !Physics.Raycast(playerCam.transform.position, Vector3.up, 1f))
        {
            StopCrouch();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching && RayCastShotgun.aiming == false)
        {
            isSprinting = true;
            maxSpeed = 7f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching)
        {
            isSprinting = false;
            maxSpeed = 5f;
        }
    }

    private void Handle_FootSteps()
    {
        if (!grounded) return;

        footStepTimer -= Time.deltaTime;
        if(footStepTimer <= 0)
        {
            if(Physics.Raycast(playerCam.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    case "Footsteps/Metal":
                        footstepAudioSource.PlayOneShot(MetalStepClips[UnityEngine.Random.Range(0, MetalStepClips.Length - 1)]);
                        break;
                    case "Footsteps/Mud":
                        footstepAudioSource.PlayOneShot(MudStepClips[UnityEngine.Random.Range(0, MudStepClips.Length - 1)]);
                        break;
                    case "Footsteps/Water":
                        footstepAudioSource.PlayOneShot(WaterStepClips[UnityEngine.Random.Range(0, WaterStepClips.Length - 1)]);
                        break;
                    default:
                        break;
                }
            }
            footStepTimer = GetCurrentOffset;
        }
    }

    private void StartCrouch()
    {
        isCrouching = true;
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);

        if (grounded)
        {
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        maxSpeed = 3f;
    }

    public void StopCrouch()
    {
        isCrouching = false;
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        maxSpeed = 5f;
    }

    private void Movement()
    {
        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;

        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        //Set max speed
        float maxSpeed = this.maxSpeed;

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        // Movement in air
        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        // Movement while sliding
        if (grounded && crouching) multiplierV = 0f;

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private float desiredX;
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * defaultFixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * defaultFixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        //Slow down sliding
        if (crouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;

    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded()
    {
        grounded = false;
    }

    private void SlowMotion()
    {
        if (Input.GetMouseButtonDown(1))
        {
            timeToggle = !timeToggle;
            Time.timeScale = timeToggle ? timeScale : defaultTimeScale;
            Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labelText);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "mixamorig:RightHand")
        {
            DamageInformation.alpha = 1;
            _gameManager.Lives -= 5;
            FindObjectOfType<AudioManager>().Play("Injured");
            StartCoroutine(DamageInfoFadeOut());
            Debug.Log("Thats Hurt");
        }
    }

    IEnumerator DamageInfoFadeOut()
    {
        if(DamageInformation.alpha > 0)
        {
            while (DamageInformation.alpha > 0)
            {
                yield return new WaitForSeconds(0.01f);
                DamageInformation.alpha -= 0.1f; 
            }
        }
    }

    /*void StepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }
        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
        {

            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.1f))
        {

            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }
    }*/

    IEnumerator LoadLevel(string levelname)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelname);
    }
}

