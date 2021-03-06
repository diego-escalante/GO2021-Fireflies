using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float moveSpeed = 4f;
    // [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float JumpHeight = 0.6f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float mouseSensitivity = 1.5f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    
    private float jumpVelocity;
    private Vector3 velocity;
    private Vector3 targetLookRotation;
    private CharacterController controller;
    private Transform camTrans;
    private float xRotation;
    private float coyoteTimeLeft;
    private float jumpBufferTimeLeft = -1;
    
    private void Awake() {
        controller = GetComponent<CharacterController>();
        camTrans = transform.Find("CameraTripod");
        jumpVelocity = Mathf.Sqrt(-2 * gravity * JumpHeight);

        // Unity's Character Controller has no concept of a LayerMask for its collision detection.
        // So we must do this instead.
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Fireflies"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Nonsolid Fireflies"));
    }
    
    private void Update() {
        // Handle body rotation. (Looking Left/Right)
        Vector3 bodyRotation = transform.eulerAngles;
        bodyRotation.y += Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.eulerAngles = bodyRotation;

        // Handle head rotation. (Looking Up/Down)
        xRotation = Mathf.Clamp(xRotation - Input.GetAxis("Mouse Y"), -80, 80);
        camTrans.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Handle grounding.
        if (controller.isGrounded) {
            coyoteTimeLeft = coyoteTime;
        }
        
        // Manage jump buffer time.
        jumpBufferTimeLeft = Input.GetButtonDown("Jump") ? jumpBufferTime : jumpBufferTimeLeft - Time.deltaTime;

        // Handle jumping.
        if (coyoteTimeLeft >= 0) {
            if (jumpBufferTimeLeft >= 0) {
                // Handle jumping if in coyote time and want to jump.
                velocity.y = jumpVelocity;
                coyoteTimeLeft = -1;
                jumpBufferTimeLeft = -1;
            } else {
                // Stay vertically still if in coyote time and not jumping.
                velocity.y = 0;
            }
        }
        
        // Handle falling.
        // TODO: This makes the player drift ever so slightly downwards while in coyote time. Not ideal, but
        // vertical velocity needs to always be applied downwards for the controller to know if it's grounded.
        velocity.y += gravity * Time.deltaTime; 
        
        // Update coyote time.
        coyoteTimeLeft -= Time.deltaTime;

        // Handle horizontal movement. (Forwards, Backwards, Strafing)
        float currentSpeed = /*Input.GetButton("Fire3") ? sprintSpeed :*/ moveSpeed;
        velocity.x = Input.GetAxisRaw("Horizontal") * currentSpeed;
        velocity.z = Input.GetAxisRaw("Vertical") * currentSpeed;
        
        // Use the character controller to move.
        controller.Move(transform.TransformVector(velocity) * Time.deltaTime);
    }

    public bool IsMoving() {
        return velocity.x != 0 || velocity.z != 0;
    }

    public bool IsGrounded() {
        return controller.isGrounded;
    }
}
