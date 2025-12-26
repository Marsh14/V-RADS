using UnityEngine;

// This script manages the individual radiation objects in the game.
public class RadiationHazard : MonoBehaviour
{
    [Header("Hazard Data")]
    public float strength = 100f; 
    private float initialStrength;

    [Header("Prefab Settings")]
    public GameObject uiPrefab;       
    public float heightOffset = 1.5f; 

    [Header("Visuals")]
    public Material safeMaterial;     

    private bool isFixed = false;
    private Renderer objRenderer;
    private AudioSource audioSource;
    private HazardManager cachedManager;
    private GameObject spawnedUI;

    void Awake()
    {
        initialStrength = strength;
        objRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        cachedManager = FindAnyObjectByType<HazardManager>();
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
    public void FixHazard()
    {
        if (isFixed) return; // If already fixed (or wasn't active), stop here.

        isFixed = true;

        // Kill Radiation
        strength = 0f;

        // Change Color
        if (objRenderer != null && safeMaterial != null)
        {
            objRenderer.material = safeMaterial;
        }

        // Spawn Success UI
        if (uiPrefab != null)
        {
            Vector3 spawnPos = transform.position + (Vector3.up * heightOffset);
            spawnedUI = Instantiate(uiPrefab, spawnPos, Quaternion.identity);

        }

        // Play Sound
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        if (cachedManager != null)
        {
            cachedManager.RegisterFixedHazard();
        }
        else
        {
            //Debug.LogWarning("No HazardManager found!");
        }
    }
    // Called when the game is done to get rid of the indivudal popups so they dont block the success canvas
    public void DestroyPopup()
    {
        if (spawnedUI != null)
        {
            Destroy(spawnedUI);
        }
    }
}