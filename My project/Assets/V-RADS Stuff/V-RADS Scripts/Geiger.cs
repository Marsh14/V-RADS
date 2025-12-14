using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Geiger : MonoBehaviour
{
    [Header("Setup")]
    private List<RadiationHazard> allHazards = new List<RadiationHazard>();
    public AudioSource audioSource;

    [Header("Geiger Feedback")]
    public float maxClickDelay = 3.0f;
    public float minClickDelay = 0.05f;

    [Tooltip("Higher number = Slower clicking for the same radiation. Try 10 or 20.")]
    public float sensitivity = 5.0f; // <--- NEW VARIABLE

    private float nextClickTime = 0f;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        RadiationHazard[] foundHazards = FindObjectsByType<RadiationHazard>(FindObjectsSortMode.None);
        allHazards.AddRange(foundHazards);
    }

    void Update()
    {
        if (grabInteractable == null || !grabInteractable.isSelected) return;

        float totalIntensity = 0f;

        foreach (RadiationHazard hazard in allHazards)
        {
            if (hazard == null) continue;

            float dist = Vector3.Distance(transform.position, hazard.transform.position);
            dist = Mathf.Max(dist, 0.1f);

            totalIntensity += hazard.strength / (dist * dist);
        }

        // --- THE FIX IS HERE ---
        if (totalIntensity <= 0.001f) totalIntensity = 0.001f;

        // Instead of 1.0f / totalIntensity, we use sensitivity / totalIntensity
        // If sensitivity is 15, the delay is 15x longer (slower clicks).
        float targetDelay = Mathf.Clamp(sensitivity / totalIntensity, minClickDelay, maxClickDelay);

        if (Time.time >= nextClickTime)
        {
            PlayClick(totalIntensity);

            float randomFactor = Random.Range(0f, targetDelay * 0.2f);
            nextClickTime = Time.time + targetDelay + randomFactor;
        }
    }

    void PlayClick(float intensity)
    {
        // Don't play if barely any radiation (prevents single random clicks far away)
        if (intensity <= 0.1f) return;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(audioSource.clip);

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