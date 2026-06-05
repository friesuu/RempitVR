using UnityEngine;

public class CharacterDataCarrier : MonoBehaviour
{
    public static CharacterDataCarrier Instance;

    // Slots to hold the index value chosen from every dropdown
    [Header("Saved Customization Indices")]
    public int savedHair;
    public int savedBengkung;
    public int savedBengkungMat;
    public int savedAccessory;
    public int savedCapalMat;
    public int savedTengkolokMat;
    public int savedShirtColor;
    public int savedHairColor;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}