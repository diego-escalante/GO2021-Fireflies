using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float JumpHeight = 0.6f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float mouseSensitivity = 200f;
    
    
    private float jumpVelocity;
    
    private Vector3 velocity;
    private Vector3 targetLookRotation;
    private CharacterController controller;
    private Transform camTrans;
    private float xRotation;
    
    private void Awake() {
        controller = GetComponent<CharacterController>();
        camTrans = transform.Find("Main Camera");
        jumpVelocity = Mathf.Sqrt(-2 * gravity * JumpHeight);

        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update() {
        
        // Handle body rotation. (Looking Left/Right)
        Vector3 bodyRotation = transform.eulerAngles;
        bodyRotation.y += Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.eulerAngles = bodyRotation;

        // Handle head rotation. (Looking Up/Down)
        xRotation = Mathf.Clamp(xRotation - Input.GetAxis("Mouse Y"), -80, 80);
        camTrans.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Handle vertical movement. (Jumping, Falling, Grounded)
        if (controller.isGrounded) {
            velocity.y = Input.GetButtonDown("Jump") ? jumpVelocity : 0;
        }
        velocity.y += gravity * Time.deltaTime;
        
        // Handle horizontal movement. (Forwards, Backwards, Strafing)
        velocity.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        velocity.z = Input.GetAxisRaw("Vertical") * moveSpeed;
        
        // Use the character controller to move.
        controller.Move(transform.TransformVector(velocity) * Time.deltaTime);
    }
}
