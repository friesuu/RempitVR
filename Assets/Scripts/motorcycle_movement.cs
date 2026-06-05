using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class motorcycle_movement : MonoBehaviour
{
    [Header("Movement")]
    public float acceleration = 3000f;
    public float maxSpeed = 20f;

    [Header("References")]
    public Rigidbody groundRb;

    [Header("Mount Settings")]
    public float requiredHoldTime = 1.0f;
    public Transform dismountPoint;
    public Transform mountPoint;
    public float dismountHeightOffset = 1.2f;

    private Rigidbody rb;
    private float moveInputV;
    private float moveInputH;
    private bool isMounted = false;
    private bool playerInRange = false;
    private GameObject playerObject;
    private float holdTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;

        if (groundRb != null)
            groundRb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void Update()
    {
        if (!isMounted && playerInRange && playerObject != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
                Mount(playerObject);
        }

        if (!isMounted) return;

        if (Keyboard.current.fKey.isPressed)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= requiredHoldTime)
                Dismount();
        }
        else
        {
            holdTimer = 0f;
        }

        moveInputV = 0f;
        moveInputH = 0f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            moveInputV = 1f;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            moveInputV = -1f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            moveInputH = -1f;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            moveInputH = 1f;
    }

    void FixedUpdate()
    {
        if (!isMounted) return;

        Vector3 input = new Vector3(moveInputH, 0f, moveInputV).normalized;

        if (input.magnitude > 0f && groundRb.linearVelocity.magnitude < maxSpeed)
            groundRb.AddForce(input * acceleration * Time.fixedDeltaTime);

        // Decelerate when no input
        if (input.magnitude == 0f)
            groundRb.linearVelocity = Vector3.Lerp(groundRb.linearVelocity, Vector3.zero, 0.1f);

        // Sync bike body to ground RB
        transform.position = groundRb.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMounted && other.CompareTag("Player"))
        {
            playerInRange = true;
            playerObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (!isMounted) playerObject = null;
        }
    }

    private void Mount(GameObject player)
    {
        isMounted = true;
        playerInRange = false;
        playerObject = player;

        groundRb.constraints = RigidbodyConstraints.FreezeRotation
                             | RigidbodyConstraints.FreezePositionY;

        Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers) r.enabled = false;

        Collider[] colliders = player.GetComponentsInChildren<Collider>();
        foreach (var c in colliders) c.enabled = false;

        MonoBehaviour playerController = player.GetComponent<MonoBehaviour>();
        if (playerController != null) playerController.enabled = false;

        if (mountPoint != null)
            player.transform.SetPositionAndRotation(mountPoint.position, mountPoint.rotation);

        Camera cam = Camera.main;
        if (cam != null)
        {
            CameraFollow follow = cam.GetComponent<CameraFollow>();
            if (follow != null) follow.target = transform;
        }
    }

    private void Dismount()
    {
        isMounted = false;
        holdTimer = 0f;
        moveInputV = 0f;
        moveInputH = 0f;

        groundRb.constraints = RigidbodyConstraints.FreezeAll;
        groundRb.linearVelocity = Vector3.zero;
        groundRb.angularVelocity = Vector3.zero;

        Vector3 exitPos = dismountPoint != null
            ? dismountPoint.position + Vector3.up * dismountHeightOffset
            : transform.position + Vector3.right * 1.5f + Vector3.up * dismountHeightOffset;

        playerObject.transform.position = exitPos;
        playerObject.transform.SetParent(null);

        Renderer[] renderers = playerObject.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers) r.enabled = true;

        Collider[] colliders = playerObject.GetComponentsInChildren<Collider>();
        foreach (var c in colliders) c.enabled = true;

        MonoBehaviour playerController = playerObject.GetComponent<SimpleController>();
        if (playerController != null) playerController.enabled = true;

        Camera cam = Camera.main;
        if (cam != null)
        {
            CameraFollow follow = cam.GetComponent<CameraFollow>();
            if (follow != null) follow.target = playerObject.transform;
        }

        playerObject = null;
    }
}