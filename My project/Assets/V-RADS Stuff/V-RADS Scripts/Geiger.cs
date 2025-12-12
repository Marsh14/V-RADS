using UnityEngine;
using System.Collections.Generic; // Required for Lists
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // Updated namespace for Unity 6

public class Geiger : MonoBehaviour
{
    [Header("Setup")]
    // We hide this list because we fill it automatically in Start()
    private List<RadiationHazard> allHazards = new List<RadiationHazard>();
    public AudioSource audioSource;

    [Header("Geiger Feedback")]
    public float maxClickDelay = 3.0f;
    public float minClickDelay = 0.05f;

    private float nextClickTime = 0f;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        // 1. FIND ALL HAZARDS AUTOMATICALLY
        // We look for every object in the scene that has the 'RadiationHazard' script
        RadiationHazard[] foundHazards = FindObjectsByType<RadiationHazard>(FindObjectsSortMode.None);

        // Add them to our main list so Update can see them
        allHazards.AddRange(foundHazards);
    }

    void Update()
    {
        // Stop if not held
        if (grabInteractable == null || !grabInteractable.isSelected) return;

        // 2. RESET TOTAL INTENSITY
        float totalIntensity = 0f;

        // 3. LOOP THROUGH HAZARDS
        foreach (RadiationHazard hazard in allHazards)
        {
            if (hazard == null) continue; // Skip if a barrel was deleted

            // Calculate distance from THIS TOOL (transform.position) to the hazard
            float dist = Vector3.Distance(transform.position, hazard.transform.position);
            dist = Mathf.Max(dist, 0.1f); // Safety clamp

            // Add this barrel's radiation to the total sum
            totalIntensity += hazard.strength / (dist * dist);
        }

        // 4. CLICK LOGIC (Using Total Intensity)
        // If totalIntensity is 0 (safe), we treat it as very low to avoid divide by zero
        if (totalIntensity <= 0.001f) totalIntensity = 0.001f;

        float targetDelay = Mathf.Clamp(1.0f / totalIntensity, minClickDelay, maxClickDelay);

        if (Time.time >= nextClickTime)
        {
            // YES! Pass totalIntensity here.
            PlayClick(totalIntensity);

            float randomFactor = Random.Range(0f, targetDelay * 0.2f);
            nextClickTime = Time.time + targetDelay + randomFactor;
        }
    }

    void PlayClick(float intensity)
    {
        if (intensity <= 0.01f) return;
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(audioSource.clip);

        // Haptics also use the total intensity
        float hapticStrength = Mathf.Clamp01(intensity / 10.0f);
        TriggerHaptics(hapticStrength);
    }

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