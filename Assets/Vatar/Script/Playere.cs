using UnityEngine;
using System.Collections;

public class Playere : MonoBehaviour
{
    [Header("Basic Movement")]
    public float moveSpeed = 5f;
    public float crouchSpeed = 2.5f;
    public float mouseSensitivity = 1f;
    public float jumpForce = 6f;
    public Transform cameraTransform;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.5f;
    public LayerMask groundMask;

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        standCamLocalPos = cameraTransform.localPosition;
        crouchCamLocalPos = standCamLocalPos + new Vector3(0, -0.4f, 0);
    }

    void Update()
    {
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

    void FixedUpdate()
    {
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
            StartCoroutine(CrouchRoutine()); // auto crouch setelah lompat jika sebelumnya crouch
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
            cameraTransform.localPosition = crouchCamLocalPos;
        }
        else
        {
            anim.SetTrigger("ToStand");
            yield return new WaitForSeconds(0.4f);
            isCrouching = false;
            cameraTransform.localPosition = standCamLocalPos;
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
