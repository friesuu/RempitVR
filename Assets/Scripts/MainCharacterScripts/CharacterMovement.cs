using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float rotationSpeed = 100f;

    private void Update()
    {
        float vertical = 0f;
        if (Keyboard.current.upArrowKey.isPressed)   vertical = 1f;
        if (Keyboard.current.downArrowKey.isPressed) vertical = -1f;

        float horizontal = 0f;
        if (Keyboard.current.leftArrowKey.isPressed)  horizontal = -1f;
        if (Keyboard.current.rightArrowKey.isPressed) horizontal = 1f;

        // Move forward/backward
        Vector3 moveDirection = transform.forward * vertical * moveSpeed * Time.deltaTime;
        transform.position += moveDirection;

        // ← Rotate around character's own Y axis at current position
        if (horizontal != 0f)
        {
            float rotation = horizontal * rotationSpeed * Time.deltaTime;
            transform.RotateAround(
                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Vector3.up,
                rotation
            );
        }
    }
}