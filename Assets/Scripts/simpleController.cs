using UnityEngine;
// We need to add this line to use the New Input System via code
using UnityEngine.InputSystem;

public class SimpleController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 0.1f;

    private CharacterController controller;
    private Camera cam;
    private float verticalRotation = 0f;
    private CarController _nearestCar;

    [Header("Interaction")]
    [Tooltip("How close the player must be to mount the car.")]
    public float interactDistance = 3f;
 
    [Tooltip("Key the player presses to enter/exit the car.")]
    public KeyCode mountKey = KeyCode.F;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. New Input System Camera Look (Mouse)
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseDelta.x);
        verticalRotation -= mouseDelta.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // 2. New Input System Movement (WASD / Arrow Keys)
        Vector2 moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveInput.y = 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveInput.y = -1;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput.x = -1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x = 1;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Apply simple gravity
        move.y = Physics.gravity.y;

        controller.Move(move * moveSpeed * Time.deltaTime);

        //carr
        DetectNearbyCar();
 
        if (_nearestCar != null && Input.GetKeyDown(mountKey))
            _nearestCar.Mount(transform);
    }

    private void DetectNearbyCar()
    {
        // Simple overlap sphere — efficient enough for most games
        Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance);
 
        CarController found = null;
        foreach (Collider col in hits)
        {
            CarController car = col.GetComponentInParent<CarController>();
            if (car != null)
            {
                found = car;
                break;
            }
        }
 
        _nearestCar = found;
    }
 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}