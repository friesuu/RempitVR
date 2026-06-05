using UnityEngine;

public class POVToggle : MonoBehaviour
{
    [Header("Camera References")]
    public GameObject firstPersonCam;
    public GameObject thirdPersonCam;

    [Header("Hotkeys")]
    public KeyCode toggleKey = KeyCode.V; // Press 'V' to switch views

    private bool isFirstPerson = true;

    void Start()
    {
        // Ensure the game starts with the correct camera active
        UpdateCameraViews();
    }

    void Update()
    {
        // Detect if the player presses the toggle key
        if (Input.GetKeyDown(toggleKey))
        {
            isFirstPerson = !isFirstPerson; // Flip the switch
            UpdateCameraViews();
        }
    }

    void UpdateCameraViews()
    {
        if (isFirstPerson)
        {
            firstPersonCam.SetActive(true);
            thirdPersonCam.SetActive(false);
        }
        else
        {
            firstPersonCam.SetActive(false);
            thirdPersonCam.SetActive(true);
        }
    }
}