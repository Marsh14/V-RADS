using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BarrelHazard : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject markUpUI;   // The "Marked for Cleaning" Canvas
    public GameObject hazardTape; // Optional: 3D barrier tape that appears
    public Renderer objectRenderer; // To change color (optional)
    public Material safeMaterial;   // The green "Success" material

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip successSound;

    private bool isMarked = false;

    void Start()
    {
        // Hide the UI and Tape at the start so the user has to find it first
        if (markUpUI != null) markUpUI.SetActive(false);
        if (hazardTape != null) hazardTape.SetActive(false);
    }

    // This function will be called by the VR Event
    public void MarkAsFound()
    {
        // Prevent marking it twice
        if (isMarked) return;

        isMarked = true;

        // 1. Show the UI
        if (markUpUI != null) markUpUI.SetActive(true);

        // 2. Show optional props (like caution tape)
        if (hazardTape != null) hazardTape.SetActive(true);

        // 3. Change color to Green (Visual feedback)
        if (objectRenderer != null && safeMaterial != null)
        {
            objectRenderer.material = safeMaterial;
        }

        // 4. Play Sound
        if (audioSource != null && successSound != null)
        {
            audioSource.PlayOneShot(successSound);
        }

        // Optional: Disable the "Hazard" logic (stop the Geiger counter?)
        // You could communicate with the Geiger script here if you wanted.
    }
}