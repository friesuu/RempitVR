using UnityEngine;

public class CharacterCustomizer : MonoBehaviour
{
    public GameObject[] hairOptions;
    public SkinnedMeshRenderer[] HairRenderer;
    private int currentHairIndex = 0;

    public GameObject[] BengkungSamping;
    public Material[] BengkungSampingMaterials;
    public SkinnedMeshRenderer[] BengkungSampingRenderer;
    private int currentBengkungSampingIndex = 0;

    public GameObject[] AccessoryOptions;
    public SkinnedMeshRenderer[] AccessoryRenderers;

    public Material[] CapalMaterials;
    public SkinnedMeshRenderer CapalRenderer;

    public Material[] TengkolokMaterials;
    public SkinnedMeshRenderer TengkolokRenderer;

    public SkinnedMeshRenderer ShirtRenderer;


   
    private Color[] shirtColors = new Color[]
    {
        new Color(1f, 1f, 1f),      // White
        new Color(0f, 0f, 0f),      // Black
        new Color(1f, 0f, 0f),      // Red
        new Color(0f, 0f, 1f),      // Blue
        new Color(0.5f, 0.3f, 0.1f) // Brown
    };

    private Color[] hairColors = new Color[]
    {
        new Color(1f, 1f, 1f),      // White
        new Color(0f, 0f, 0f),      // Black
        new Color(0.95f, 0.85f, 0.4f), // Blonde
        new Color(0.7f, 0.7f, 0.7f),   // Grey
        new Color(0.5f, 0.3f, 0.1f) // Brown
    };

    public void SelectHair(int index)
    {
        currentHairIndex = index; // save current hair
        Debug.Log("Dropdown selected: " + index);
        for (int i = 0; i < hairOptions.Length; i++)
        {
            hairOptions[i].SetActive(i == index);
        }
    }

    public void SelectBengkungSamping(int index)
    {
        currentBengkungSampingIndex = index; // save current bengkung samping
        for (int i = 0; i < BengkungSamping.Length; i++)
        {
            BengkungSamping[i].SetActive(i == index);
        }
    }


    public void SelectBengkungSampingMaterial(int index)
{
    // Apply material to currently active bengkung samping only
    if (currentBengkungSampingIndex < BengkungSampingRenderer.Length)
    {
        BengkungSampingRenderer[currentBengkungSampingIndex].material = BengkungSampingMaterials[index];
    }
}

    public void SelectAccessory(int index)
    {
        if (index == 0)
        {
            for (int i = 0; i < AccessoryOptions.Length; i++)
            {
                AccessoryOptions[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < AccessoryOptions.Length; i++)
            {
                AccessoryOptions[i].SetActive(i == index - 1);
            }
        }
    }

    public void SelectCapalMaterial(int index)
{
    CapalRenderer.material = CapalMaterials[index];
}

public void SelectTengkolokMaterial(int index)
{
    TengkolokRenderer.material = TengkolokMaterials[index];
}


    public void SetShirtColorByIndex(int index)
    {
        ShirtRenderer.material.color = shirtColors[index];
    }

    public void SetHairColorByIndex(int index)
    {
        // ← Apply color to ALL hair renderers
        foreach (SkinnedMeshRenderer renderer in HairRenderer)
        {
            if (renderer != null)
                renderer.material.color = hairColors[index];
        }
    }

    public void SetAccessoryYellow() // ← Always yellow, no dropdown needed
    {
        Color yellow = new Color(1f, 1f, 0f);

        foreach (SkinnedMeshRenderer renderer in AccessoryRenderers)
        {
            if (renderer != null)
                renderer.material.color = yellow;
        }
    }

    private void Start()
    {
        SelectHair(2);
        SelectAccessory(0);
        SetShirtColorByIndex(0);
        SetHairColorByIndex(0);
        SelectCapalMaterial(0);
        SelectBengkungSamping(0);
        SelectBengkungSampingMaterial(0);
        SelectTengkolokMaterial(0);
        SetAccessoryYellow();
    }
}