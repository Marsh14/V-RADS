using UnityEngine;

public class RadiationHazard : MonoBehaviour
{
    [Header("Hazard Data")]
    public float strength = 100f; // <--- This was missing! 
    private float initialStrength;

    [Header("Prefab Settings")]
    public GameObject uiPrefab;       // Drag your "Success Canvas" Prefab here
    public float heightOffset = 1.5f; // How high to spawn the UI

    [Header("Visuals")]
    public Material safeMaterial;     // The Green Material

    private bool isFixed = false;
    private Renderer objRenderer;
    private AudioSource audioSource;

    void Awake()
    {
        initialStrength = strength;
        objRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Called by the Geiger Counter / Health Script to randomize the game
    public void SetActiveHazard(bool isActive)
    {
        if (isActive)
        {
            strength = initialStrength;
            isFixed = false;
        }
        else
        {
            strength = 0f;
            isFixed = true;
        }
    }

    // Called when you click/select the barrel
    public void FixHazard()
    {
        if (isFixed) return;
        isFixed = true;

        // 1. Kill Radiation
        strength = 0f;

        // 2. Change Color
        if (objRenderer != null && safeMaterial != null)
        {
            objRenderer.material = safeMaterial;
        }

        // 3. Spawn the Success UI
        if (uiPrefab != null)
        {
            Vector3 spawnPos = transform.position + (Vector3.up * heightOffset);
            Instantiate(uiPrefab, spawnPos, Quaternion.identity);
        }

        // 4. Play Sound (Optional)
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        Debug.Log("Hazard Neutralized!");
    }
}