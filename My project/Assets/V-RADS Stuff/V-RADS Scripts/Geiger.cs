using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// This script simulates a Geiger counter that clicks faster with higher radiation levels
public class Geiger : MonoBehaviour
{
    [Header("Setup")]
    private List<RadiationHazard> allHazards = new List<RadiationHazard>();
    public AudioSource audioSource;

    [Header("Geiger Feedback")]
    public float maxClickDelay = 3.0f;
    public float minClickDelay = 0.05f;

    [Tooltip("Higher number = Slower clicking for the same radiation. Try 10 or 20.")]
    public float sensitivity = 5.0f; 

    private float nextClickTime = 0f;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        // Get reference to XRGrabInteractable
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        // Find all RadiationHazard objects in the scene
        RadiationHazard[] foundHazards = FindObjectsByType<RadiationHazard>(FindObjectsSortMode.None);
        allHazards.AddRange(foundHazards);
    }

    void Update()
    {
        if (grabInteractable == null || !grabInteractable.isSelected) return;

        float totalIntensity = 0f;

        // Calculate total radiation intensity at this position
        foreach (RadiationHazard hazard in allHazards)
        {
            if (hazard == null) continue;

            // Calculate distance to hazard, makes sure we don't divide by zero
            float dist = Vector3.Distance(transform.position, hazard.transform.position);
            dist = Mathf.Max(dist, 0.1f);

            // Inverse square law for radiation intensity
            totalIntensity += hazard.strength / (dist * dist);
        }

        // Prevent division by zero
        if (totalIntensity <= 0.001f) totalIntensity = 0.001f;

        // Determine target delay based on intensity and sensitivity
        float targetDelay = Mathf.Clamp(sensitivity / totalIntensity, minClickDelay, maxClickDelay);

        if (Time.time >= nextClickTime)
        {
            // Play click sound and haptics
            PlayClick(totalIntensity);

            float randomFactor = Random.Range(0f, targetDelay * 0.2f);
            nextClickTime = Time.time + targetDelay + randomFactor;
        }
    }

    void PlayClick(float intensity)
    {
        // Don't play if barely any radiation (prevents single random clicks far away)
        if (intensity <= 0.1f) return;

        // Randomize pitch slightly for variety
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(audioSource.clip);

        float hapticStrength = Mathf.Clamp01(intensity / 10.0f);
        TriggerHaptics(hapticStrength);
    }

    // Triggers haptic feedback on the controller holding the Geiger counter
    void TriggerHaptics(float strength)
    {
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            var hand = grabInteractable.interactorsSelecting[0];
            if (hand is XRBaseInputInteractor controllerInteractor)
            {
                controllerInteractor.SendHapticImpulse(strength, 0.1f);
            }
        }
    }
}