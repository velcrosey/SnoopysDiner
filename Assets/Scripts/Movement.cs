using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    private Rigidbody rb;
    private Animator animator;
    private float verticalLookRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set initial drag values for motionless state
        rb.drag = 5f;
        rb.angularDrag = 5f;

        animator.SetInteger("AnimState", 0);
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Check horizontal input for rotating the character
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        transform.Rotate(Vector3.up * mouseX * Time.deltaTime * sensitivity);

        // Check if "W" key is pressed
        if (Input.GetKey(KeyCode.W))
        {
            MoveForward();
        }
        else
        {
            StopMoving();
        }
    }

    private void MoveForward()
    {
        // Apply force to move the character forward
        Vector3 moveDirection = transform.forward;
        rb.AddForce(moveDirection * speed, ForceMode.VelocityChange);

        // Set movement animation state
        animator.SetInteger("AnimState", 1);

        // Trigger footstep
        OnFootstep();
    }

    private void StopMoving()
    {
        // Reset animation state
        animator.SetInteger("AnimState", 0);

        // Stop character's velocity
        rb.velocity = Vector3.zero;
    }

    public void OnFootstep()
    {
        // Implement footstep logic here
    }
}
