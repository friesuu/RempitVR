using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // Your character
    public float followSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 3f, -6f); // Behind and above

    private void LateUpdate()
    {
        if (target == null) return;

        // Follow position
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Always look at character
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}