using UnityEngine;

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
    // NEW: Reference to the radiation data so we can turn it off
    public RadiationHazard radiationHazard;

    private bool isMarked = false;

    void Start()
    {
        if (markUpUI != null) markUpUI.SetActive(false);
        if (hazardTape != null) hazardTape.SetActive(false);

        // Auto-find the hazard script if we forgot to drag it in
        if (radiationHazard == null)
        {
            radiationHazard = GetComponent<RadiationHazard>();
        }
    }

    public void MarkAsFound()
    {
        if (isMarked) return;
        isMarked = true;

        // 1. VISUALS (Show UI, Tape, etc)
        if (markUpUI != null) markUpUI.SetActive(true);
        if (hazardTape != null) hazardTape.SetActive(true);

        if (objectRenderer != null && safeMaterial != null)
        {
            objectRenderer.material = safeMaterial;
        }

        // 2. AUDIO
        if (audioSource != null && successSound != null)
        {
            audioSource.PlayOneShot(successSound);
        }

        // 3. THE FIX: KILL THE RADIATION
        if (radiationHazard != null)
        {
            // Setting this to 0 means:
            // Intensity = 0 / Distance^2 = 0.
            // Result: No damage, no clicking.
            radiationHazard.strength = 0f;
        }
    }
}