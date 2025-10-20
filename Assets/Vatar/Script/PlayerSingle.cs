using UnityEngine;
using System.Collections;
using Photon.Pun;
using cakeslice;
using UnityEditor;
using UnityEngine.Rendering.PostProcessing;

public class PlayerSingle : MonoBehaviourPunCallbacks
{
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

    /*[Header("Canvas Inventory")]
    public GameObject ArghaInventory;
    public GameObject IrulInventory;*/

    [Header("Mini Map")]
    public GameObject CanvasMap;
    public GameObject CanvasMap2;
    public GameObject markPlayer1;
    public GameObject markPlayer2;
    public GameObject arrow1;
    public GameObject arrow2;
    public bool showMap1;

    private AudioSource walkSource;
    private AudioSource sfxSource;

    private Rigidbody rb;
    private Animator anim;

    private bool isGrounded;
    private bool isCrouching = false;
    private bool isCrouchTransitioning = false;
    private bool isJumpingFromCrouch = false;

    private Vector3 standCamLocalPos;
    private Vector3 crouchCamLocalPos;
    private float xRotation = 0f;

    private OutlineEffect outlineEffect;

    //public GameObject canvasInventory;

    private KeyCode crouchKey = KeyCode.C;

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


        /*if (photonView.IsMine)
        {
            canvasInventory.SetActive(true);
        }*/


        /*ArghaInventory = GameObject.FindGameObjectWithTag("ArghaInventory");
        IrulInventory = GameObject.FindGameObjectWithTag("IrulInventory");*/

        /*if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("ChosenCharacter"))
        {
            string chosenChar = PhotonNetwork.LocalPlayer.CustomProperties["ChosenCharacter"].ToString();

            if (chosenChar == "Karakter1" && photonView.IsMine)
            {
                ArghaInventory.SetActive(true);
                IrulInventory.SetActive(false);
            }
            else if (chosenChar == "Karakter2" && photonView.IsMine)
            {
                ArghaInventory.SetActive(false);
                IrulInventory.SetActive(true);
            }
        }*/

        /*if (!photonView.IsMine)
        {
            canvasInventory.SetActive(false);
        }
        else
        {
            canvasInventory.SetActive(true);
        }*/
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

        /*if (!photonView.IsMine)
        {
            // Nonaktifkan komponen yang hanya boleh untuk local player
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }*/
    }

    void Update()
    {
        if (PauseManager.GameIsPaused) return;

        HoldingItemHand();
        LookAround();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(crouchKey))
        {
            StartCoroutine(CrouchRoutine());
        }

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


        arrow1.SetActive(markPlayer1);

        ToggleMarker(showMap1);

        UpdateAnimation();
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

    IEnumerator CrouchRoutine()
    {
        if (isCrouchTransitioning) yield break;
        isCrouchTransitioning = true;

        if (!isCrouching)
        {
            anim.SetTrigger("ToCrouch");
            yield return new WaitForSeconds(0.4f);
            isCrouching = true;
        }
        else
        {
            anim.SetTrigger("ToStand");
            yield return new WaitForSeconds(0.4f);
            isCrouching = false;
        }

        isCrouchTransitioning = false;
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

        if (isGrounded && !walkSource.isPlaying && speed > 0.1f && sfxVolume > 0f)
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