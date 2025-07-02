using UnityEngine;

public class Playere : MonoBehaviour
{
    [Header("Basic Movement")]
    public float moveSpeed = 4f;
    public float mouseSensitivity = 1f;
    public float jumpForce = 5f;
    public Transform cameraTransform;

    [Header("Jongkok")]
    public float jongkok = 1f;
    public float berdiri = 2f;
    public float crouchSpeed = 2.5f;
    public float standSpeed = 5f;
    private Vector3 crouchCamLocalPos;
    private Vector3 standCamLocalPos;
    private KeyCode crouchKey = KeyCode.C;

    private Rigidbody rb;
    private float xRotation = 0f;
    public LayerMask groundMask;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    private bool isGrounded;

    private bool isCrouching = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        LookAround();
        Jump();
        if (Input.GetKeyDown(crouchKey))
        {
            ToggleCrouch();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 moveVelocity = move * moveSpeed;

        Vector3 velocity = rb.velocity;
        velocity.x = moveVelocity.x;
        velocity.z = moveVelocity.z;
        rb.velocity = velocity;
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reset Y
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void ToggleCrouch()
    {
        isCrouching = !isCrouching;

        if (isCrouching)
        {
            GetComponent<CapsuleCollider>().height = jongkok;
            moveSpeed = crouchSpeed;
            cameraTransform.localPosition = crouchCamLocalPos;
        }
        else
        {
            GetComponent<CapsuleCollider>().height = berdiri;
            moveSpeed = standSpeed;
            cameraTransform.localPosition = standCamLocalPos;
        }
    }

}
