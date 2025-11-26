using UnityEngine;
using System.Collections;
using Photon.Pun;
using cakeslice;
using UnityEditor;
using UnityEngine.Rendering.PostProcessing;

public class PlayerSingle : MonoBehaviourPunCallbacks
{
    public static PlayerSingle instance;
    [Header("Basic Movement")]
    public float moveSpeed = 5f;
    public float crouchSpeed = 2.5f;
    public float mouseSensitivity = 1f;
    public float jumpForce = 6f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.5f;
    public LayerMask groundMask;

    [Header("Audio")]
    public AudioClip walkClip;
    public AudioClip jumpClip;

    [Header("Camera Follow")]
    public Transform cameraTransform;
    public Transform cameraFollowTarget;
    public Vector3 cameraOffset = new Vector3(0f, 0f, 0f);
    public float cameraFollowSpeed = 5f;

    public PostProcessVolume postVolume;
    public PostProcessProfile profileBlurEffect;
    public PostProcessProfile profileNormal;

    [Header("Mini Map")]
    public GameObject CanvasMap;
    public GameObject CanvasMap2;
    public GameObject markPlayer1;
    public GameObject markPlayer2;
    public bool showMap1;

    [Header("Crouch Collider")]
    [SerializeField] private Collider coliderBerdiri;
    [SerializeField] private Collider coliderJongkok;

    private AudioSource walkSource;
    private AudioSource sfxSource;

    private Rigidbody rb;
    private Animator anim;

    private bool isGrounded;
    private bool isCrouching = false;
    private bool isCrouchTransitioning = false;
    private bool isJumpingFromCrouch = false;
    public bool canWalk = true;
    private bool lastCanWalkState;

    private Vector3 standCamLocalPos;
    private Vector3 crouchCamLocalPos;
    private float xRotation = 0f;

    private OutlineEffect outlineEffect;

    private KeyCode crouchKey = KeyCode.C;
    public GameObject cantMoveobj;
    public GameObject soundNonActive;

    private void Awake()
    {
        /*CanvasMap = GameObject.Find("Minimap");
        CanvasMap2 = GameObject.Find("Minimap2");*/

        //postLayer = Camera.main.GetComponent<PostProcessLayer>();

        if (CanvasMap != null ||  CanvasMap2 != null)
        {
            CanvasMap.SetActive(false);
            CanvasMap2.SetActive(false);
        }
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        walkSource = gameObject.AddComponent<AudioSource>();
        walkSource.loop = true;
        walkSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        standCamLocalPos = cameraTransform.localPosition;
        crouchCamLocalPos = standCamLocalPos + new Vector3(0, -0.4f, 0);

        OutlineEffect outlineEffect = cameraTransform.GetComponent<OutlineEffect>();
    }

    void Update()
    {
        if (PauseManager.GameIsPaused) return;
        canmoveStatus();
        UpdateAnimation();

        if (!canWalk) return;
        HoldingItemHand();
        LookAround();
        CursorStatus();
        CrouchStatus();
        Jump();
        MapStatus();
        ToggleMarker(showMap1);

        if (Input.GetKeyDown(crouchKey))
        {
            StartCoroutine(CrouchRoutine());
        }
    }

    void canmoveStatus()
    {
        bool newState = !cantMoveobj.activeInHierarchy;

        if (newState != lastCanWalkState)
        {
            canWalk = newState;
        }

        lastCanWalkState = newState;
    }


    void CursorStatus()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void HoldingItemHand()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            anim.SetBool("Holding", true);
        }
    }

    void ToggleMarker(bool showMap1)
    {
        if (showMap1)
        {
            markPlayer1.SetActive(true);
        }
        else
        {
            markPlayer1.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lantai 1"))
        {
            showMap1 = true;
        }

        if (other.CompareTag("Lantai 2"))
        {
            showMap1 = false;
        }
    }

    void LateUpdate()
    {
        if (PauseManager.GameIsPaused) return;

        SmoothFollowCamera();
    }

    void SmoothFollowCamera()
    {
        if (cameraTransform == null || cameraFollowTarget == null) return;

        Vector3 targetPosition = cameraFollowTarget.position + cameraOffset;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, cameraFollowSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (PauseManager.GameIsPaused) return;

        CheckGround();

        if (!canWalk) return;
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        float speed = isCrouching ? crouchSpeed : moveSpeed;

        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = move * speed;
        rb.velocity = new Vector3(targetVelocity.x, currentVelocity.y, targetVelocity.z);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void CheckGround()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        anim.SetBool("IsJumping", !isGrounded);

        if (!wasGrounded && isGrounded && isJumpingFromCrouch)
        {
            isJumpingFromCrouch = false;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpFunction();
        }
    }
    void JumpFunction()
    {
        if (!isGrounded || isCrouchTransitioning) return;

        if (isCrouching)
        {
            isCrouching = false;
            isJumpingFromCrouch = true;
            anim.SetTrigger("ToStand");
        }

        Vector3 jumpVelocity = rb.velocity;
        jumpVelocity.y = 0f;
        rb.velocity = jumpVelocity;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (AudioManager.Instance != null && jumpClip != null)
        {
            sfxSource.PlayOneShot(jumpClip, AudioManager.Instance.sfxVolume);
        }
    }

    void MapStatus()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (showMap1)
            {
                CanvasMap.SetActive(!CanvasMap.activeSelf);
                CanvasMap2.SetActive(false);
            }
            else
            {
                CanvasMap2.SetActive(!CanvasMap2.activeSelf);
                CanvasMap.SetActive(false);
            }
        }

        if (CanvasMap.activeInHierarchy || CanvasMap2.activeInHierarchy)
        {
            postVolume.profile = profileBlurEffect;
        }
        else
        {
            postVolume.profile = profileNormal;
        }
    }

    IEnumerator CrouchRoutine()
    {
        if (isCrouchTransitioning) yield break;
        isCrouchTransitioning = true;

        if (!isCrouching)
        {
            anim.SetTrigger("ToCrouch");
            yield return new WaitForSeconds(1f);
            isCrouching = true;
        }
        else
        {
            anim.SetTrigger("ToStand");
            yield return new WaitForSeconds(1f);
            isCrouching = false;
        }   

        yield return new WaitForSeconds(0.7f);

        isCrouchTransitioning = false;
    }

    void CrouchStatus()
    {
        if (isCrouching)
        {
            coliderBerdiri.enabled = false;
            coliderJongkok.enabled = true;
        }
        else
        {
            coliderBerdiri.enabled = true;
            coliderJongkok.enabled = false;
        }
    }

    void UpdateAnimation()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        float speed = new Vector2(inputX, inputZ).magnitude;

        anim.SetFloat("MoveSpeed", speed);
        anim.SetBool("IsCrouching", isCrouching);

        float sfxVolume = AudioManager.Instance != null ? AudioManager.Instance.sfxVolume : 1f;
        walkSource.volume = sfxVolume;

        if (isGrounded && !walkSource.isPlaying && speed > 0.1f && sfxVolume > 0f && canWalk && !soundNonActive.activeInHierarchy)
        {
            walkSource.clip = walkClip;
            walkSource.Play();
        }
        else if (speed <= 0.1f || !isGrounded || sfxVolume <= 0f)
        {
            if (walkSource.clip == walkClip)
                walkSource.Stop();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}