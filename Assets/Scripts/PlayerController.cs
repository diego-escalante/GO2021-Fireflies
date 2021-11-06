using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public Vector2 mouseSensitivity = new Vector2(200, 200);
    public float moveSpeed = 12f;
    
    private Transform cameraTrans;
    private float xRotation = 0;
    private CharacterController controller;
    
    private void Start() {
        controller = GetComponent<CharacterController>();
        cameraTrans = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update() {
        HandleLookInput();
        HandleMoveInput();
    }

    private void HandleMoveInput() {
        Vector2 wasdInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 move = transform.right * wasdInput.x + transform.forward * wasdInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    private void HandleLookInput() {
        Vector2 mouseInput = 
            new Vector2(
                Input.GetAxis("Mouse X") * mouseSensitivity.x, 
                Input.GetAxis("Mouse Y") * mouseSensitivity.y) * 
            Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation - mouseInput.y, -85, 85);
        cameraTrans.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseInput.x);
    }
    
}
