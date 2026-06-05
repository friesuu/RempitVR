using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Change to 'using UnityEngine.UI;' if using standard legacy dropdowns

public class MainMenuController : MonoBehaviour
{
    [Header("Connect Hadif's UI Dropdowns Here")]
    public TMP_Dropdown hairDropdown;
    public TMP_Dropdown bengkungDropdown;
    public TMP_Dropdown bengkungMatDropdown;
    public TMP_Dropdown accessoryDropdown;
    public TMP_Dropdown capalMatDropdown;
    public TMP_Dropdown tengkolokMatDropdown;
    public TMP_Dropdown shirtColorDropdown;
    public TMP_Dropdown hairColorDropdown;

    public string gameplaySceneName = "MainScene"; // Type your exact gameplay scene name here

    public void OnStartButtonClicked()
    {
        //CharacterDataCarrier.Instance = FindFirstObjectByType<CharacterDataCarrier>();
        if (CharacterDataCarrier.Instance != null)
        {
            // Gather all indices from the UI dropdowns
            CharacterDataCarrier.Instance.savedHair = hairDropdown.value;
            CharacterDataCarrier.Instance.savedBengkung = bengkungDropdown.value;
            CharacterDataCarrier.Instance.savedBengkungMat = bengkungMatDropdown.value;
            CharacterDataCarrier.Instance.savedAccessory = accessoryDropdown.value;
            CharacterDataCarrier.Instance.savedCapalMat = capalMatDropdown.value;
            CharacterDataCarrier.Instance.savedTengkolokMat = tengkolokMatDropdown.value;
            CharacterDataCarrier.Instance.savedShirtColor = shirtColorDropdown.value;
            CharacterDataCarrier.Instance.savedHairColor = hairColorDropdown.value;
        }

        // Fix the mouse cursor so you can play immediately
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Load your main level
        SceneManager.LoadScene(gameplaySceneName);
    }
}
