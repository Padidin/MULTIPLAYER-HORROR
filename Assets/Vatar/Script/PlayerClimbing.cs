using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float climbSpeed = 3f;

    private CharacterController controller;
    private Animator anim;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isClimbing = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Cek tanah
        isGrounded = controller.isGrounded;

        if (isClimbing)
        {
            HandleClimb();
            return; // Stop Update agar tidak ikut gravity/jump
        }

        HandleMovement();
        ApplyGravity();
        UpdateAnimation();
    }

    void HandleClimb()
    {
        float climbInput = Input.GetAxis("Vertical");

        Vector3 climbMove = new Vector3(0, climbInput * climbSpeed, 0);
        controller.Move(climbMove * Time.deltaTime);

        // Disable gravity saat climbing
        velocity.y = 0f;

        // Animasi climbing
        anim.SetBool("isClimbing", Mathf.Abs(climbInput) > 0.1f);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float forward = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * forward;
        controller.Move(move * walkSpeed * Time.deltaTime);

        // Lompat
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateAnimation()
    {
        anim.SetBool("isClimbing", false); // reset kalau nggak climbing
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            velocity = Vector3.zero; // reset velocity biar ga loncat
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            anim.SetBool("isClimbing", false);
        }
    }
}
