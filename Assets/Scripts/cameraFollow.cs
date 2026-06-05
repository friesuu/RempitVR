using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(0f, 2.5f, -6f);
    public float positionSmoothTime = 0.1f;
    public float rotationSmoothTime = 0.1f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position based on target's orientation + offset
        Vector3 desiredPosition = target.position
            + target.rotation * offset;

        // Smoothly move camera to desired position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            positionSmoothTime
        );

        // Smoothly rotate camera to look at target
        Quaternion desiredRotation = Quaternion.LookRotation(
            target.position - transform.position
        );
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSmoothTime
        );
    }
}