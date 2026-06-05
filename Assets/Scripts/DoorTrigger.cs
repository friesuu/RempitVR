using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Force the mouse cursor to become visible again
        Cursor.visible = true;

        // 2. Unlock the cursor from the center of the game screen
        Cursor.lockState = CursorLockMode.None;

        // 3. Load the menu scene safely
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}