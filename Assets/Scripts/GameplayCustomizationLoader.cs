using UnityEngine;

public class GameplayCustomizationLoader : MonoBehaviour
{
    void Start()
    {
        // 1. Find Hadif's customizer script attached to this player
        CharacterCustomizer customizer = GetComponent<CharacterCustomizer>();

        if (customizer != null && CharacterDataCarrier.Instance != null)
        {
            // 2. Feed the saved data directly back into Hadif's custom functions!
            customizer.SelectHair(CharacterDataCarrier.Instance.savedHair);
            customizer.SelectBengkungSamping(CharacterDataCarrier.Instance.savedBengkung);
            customizer.SelectBengkungSampingMaterial(CharacterDataCarrier.Instance.savedBengkungMat);
            customizer.SelectAccessory(CharacterDataCarrier.Instance.savedAccessory);
            customizer.SelectCapalMaterial(CharacterDataCarrier.Instance.savedCapalMat);
            customizer.SelectTengkolokMaterial(CharacterDataCarrier.Instance.savedTengkolokMat);
            customizer.SetShirtColorByIndex(CharacterDataCarrier.Instance.savedShirtColor);
            customizer.SetHairColorByIndex(CharacterDataCarrier.Instance.savedHairColor);

            // Hadif's accessory coloring function
            customizer.SetAccessoryYellow();
        }
    }
}