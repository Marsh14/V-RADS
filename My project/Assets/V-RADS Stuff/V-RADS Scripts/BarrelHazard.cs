using UnityEngine;

// This script was a first prototype for hazards (not used anymore)
public class BarrelHazard : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject markUpUI;
    public GameObject hazardTape;
    public Renderer objectRenderer;
    public Material safeMaterial;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip successSound;

    [Header("Logic")]
    public RadiationHazard radiationHazard;

    private bool isMarked = false;

    void Start()
    {
        if (markUpUI != null) markUpUI.SetActive(false);
        if (hazardTape != null) hazardTape.SetActive(false);

        
        if (radiationHazard == null)
        {
            radiationHazard = GetComponent<RadiationHazard>();
        }
    }

    public void MarkAsFound()
    {
        // Function to run when it was selected
        if (isMarked) return;
        isMarked = true;

        
        if (markUpUI != null) markUpUI.SetActive(true);
        if (hazardTape != null) hazardTape.SetActive(true);

        // Change Material to Safe
        if (objectRenderer != null && safeMaterial != null)
        {
            objectRenderer.material = safeMaterial;
        }

        // Play Sound
        if (audioSource != null && successSound != null)
        {
            audioSource.PlayOneShot(successSound);
        }

        // Disable Radiation
        if (radiationHazard != null)
        {
            // Setting this to 0 means:
            // Intensity = 0 / Distance^2 = 0.
            // Result: No damage, no clicking.
            radiationHazard.strength = 0f;
        }
    }
}