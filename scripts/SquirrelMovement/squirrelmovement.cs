using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SquirrelMovement : MonoBehaviour
{
    public bool isDrunk = false;
    public float walkSpeed = 5f;
    public float runSpeed = 15f;
    public float rotationSpeed = 720f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Animator animator;
    private Transform cameraTransform;
    public Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (!controller.enabled) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float speedMultiplier = isDrunk ? 0.5f : 1f;

        if (isDrunk)
        {
            h = -h;
            v = -v;
        }

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        Vector3 inputDirection = new Vector3(h, 0, v);
        float currentSpeed = (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed) * speedMultiplier;

        if (inputDirection.magnitude > 0.1f)
        {
            Vector3 moveDirection = cameraTransform.right * h + cameraTransform.forward * v;
            moveDirection.y = 0;
            moveDirection.Normalize();

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
