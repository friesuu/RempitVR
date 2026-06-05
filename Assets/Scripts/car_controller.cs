using UnityEngine;

/// <summary>
/// Simple car controller with mount/dismount support.
///
/// Setup:
///   - Attach this script to the ROOT GameObject (the one with Rigidbody).
///   - Car mesh should be a child of the root.
///   - Assign a "MountPoint" child Transform (where the player sits).
///   - Assign a "DismountPoint" child Transform (where the player appears after exiting).
///   - Tag this GameObject as "Car" (optional, for player raycasting).
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Maximum forward/backward force applied to the car.")]
    public float motorForce = 1500f;

    [Tooltip("How sharply the car turns (torque around Y axis).")]
    public float steerTorque = 600f;

    [Tooltip("Drag applied when no input is given, to slow the car down.")]
    public float brakeDrag = 5f;

    [Header("References")]
    [Tooltip("Transform where the player is parented while driving.")]
    public Transform mountPoint;

    [Tooltip("Transform where the player is placed after dismounting.")]
    public Transform dismountPoint;

    [Header("Input (Edit → Project Settings → Input Manager)")]
    public string verticalAxis  = "Vertical";
    public string horizontalAxis = "Horizontal";
    public KeyCode mountKey     = KeyCode.F;

    // ── Internal state ─────────────────────────────────────────────────────────
    private Rigidbody   _rb;
    private bool        _isDriving = false;
    private Transform   _driver;            // reference to the player Transform
    private float       _defaultDrag;

    // ── Unity lifecycle ────────────────────────────────────────────────────────

    private void Awake()
    {
        _rb          = GetComponent<Rigidbody>();
        _defaultDrag = _rb.linearDamping;

        // Freeze rotation so the box doesn't tumble (adjust as needed)
        _rb.constraints = RigidbodyConstraints.FreezeRotationX |
                          RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        // Only the driver can dismount from inside the car
        if (_isDriving && Input.GetKeyDown(mountKey))
            Dismount();
    }

    private void FixedUpdate()
    {
        if (!_isDriving) return;

        float throttle = Input.GetAxis(verticalAxis);
        float steer    = Input.GetAxis(horizontalAxis);

        // ── Throttle ──────────────────────────────────────────────────────────
        Vector3 force = transform.forward * (throttle * motorForce);
        _rb.AddForce(force, ForceMode.Force);

        // ── Steering (only when moving) ───────────────────────────────────────
        if (Mathf.Abs(throttle) > 0.05f)
        {
            float direction = Mathf.Sign(throttle); // reverse steering when reversing
            _rb.AddTorque(transform.up * (steer * steerTorque * direction), ForceMode.Force);
        }

        // ── Braking drag ──────────────────────────────────────────────────────
        _rb.linearDamping = (Mathf.Approximately(throttle, 0f)) ? brakeDrag : _defaultDrag;
    }

    // ── Public API ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Call this from the player script when they press the mount key near the car.
    /// Pass the player's Transform so it can be repositioned on mount/dismount.
    /// </summary>
    public void Mount(Transform player)
    {
        if (_isDriving) return;

        _driver    = player;
        _isDriving = true;

        // Disable player movement & physics while driving
        DisablePlayer(player, true);

        // Seat the player at the mount point
        player.SetParent(mountPoint);
        player.localPosition = Vector3.zero;
        player.localRotation = Quaternion.identity;

        Debug.Log("[CarController] Player mounted.");
    }

    /// <summary>
    /// Ejects the player from the car and re-enables their controller.
    /// </summary>
    public void Dismount()
    {
        if (!_isDriving || _driver == null) return;

        _isDriving = false;
        _rb.linearDamping   = _defaultDrag;

        // Place player at the dismount point in world space
        _driver.SetParent(null);
        _driver.position = dismountPoint != null
            ? dismountPoint.position
            : transform.position + transform.right * 2f; // fallback: 2 m to the right

        _driver.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        // Re-enable player movement & physics
        DisablePlayer(_driver, false);

        _driver = null;

        Debug.Log("[CarController] Player dismounted.");
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    /// <summary>
    /// Toggles player components that should be inactive while driving.
    /// Extend this method to match your actual player setup.
    /// </summary>
    private void DisablePlayer(Transform player, bool disable)
    {
        // ── Rigidbody ──────────────────────────────────────────────────────────
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.isKinematic = disable;
        }

        // ── Collider ───────────────────────────────────────────────────────────
        Collider playerCol = player.GetComponent<Collider>();
        if (playerCol != null)
        {
            playerCol.enabled = !disable;
        }

        // ── Player movement script ─────────────────────────────────────────────
        // Replace "PlayerController" with whatever your movement MonoBehaviour is named.
        MonoBehaviour playerMovement = player.GetComponent<MonoBehaviour>();
        if (playerMovement != null && playerMovement.GetType().Name == "PlayerController")
        {
            playerMovement.enabled = !disable;
        }
    }
}