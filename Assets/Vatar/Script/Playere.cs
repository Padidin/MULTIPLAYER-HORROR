using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Playere : MonoBehaviourPunCallbacks
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

    private KeyCode crouchKey = KeyCode.C;

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

        if (!photonView.IsMine)
        {
            cameraTransform.gameObject.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (!photonView.IsMine || PauseManager.GameIsPaused) return;

        LookAround();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(crouchKey))
        {
            StartCoroutine(CrouchRoutine());
        }

        UpdateAnimation();
    }

    void LateUpdate()
    {
        if (!photonView.IsMine || PauseManager.GameIsPaused) return;

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
        if (!photonView.IsMine || PauseManager.GameIsPaused) return;

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
