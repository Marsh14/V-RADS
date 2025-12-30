using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;

// This script simulates a Geiger counter that clicks faster with higher radiation levels
public class Geiger : MonoBehaviour
{
    [Header("Setup")]
    private List<RadiationHazard> allHazards = new List<RadiationHazard>();
    public AudioSource audioSource;

    [Header("Geiger Feedback")]
    public float maxClickDelay = 3.0f;
    public float minClickDelay = 0.05f;

    [Tooltip("Higher number = Slower clicking for the same radiation")]
    public float sensitivity = 5.0f; 

    private float nextClickTime = 0f;
    private XRGrabInteractable grabInteractable;
    public TMP_Text screenText;

    private float currentDisplayValue = 0f;

    [Header("Calibration")]
    [Tooltip("Multiplier to convert game units to uSv/h")]
    public float uSvMultiplier = 100.0f;

    [Tooltip("How fast the screen updates. Lower = more laggy/realistic")]
    public float sensorResponsiveness = 2.0f;

    public TMP_Text barGraphText; 

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
        // Calculate a fake radiation number based on click speed or distance
        // Example: If 10 clicks per second, show "500". If 0, show "0.05".
        float radiationValue = (totalIntensity * 12.5f) + Random.Range(0.01f, 0.05f);

        if (screenText != null)
        {
            // Convert "Game Intensity" to "Real World uSv/h"
            float targetValue = totalIntensity * uSvMultiplier;

            // Add "Sensor Noise" (Real sensors fluctuate slightly)
            float noise = Random.Range(0.95f, 1.05f);
            targetValue *= noise;

            // Smooth the value (Linear Interpolation)
            currentDisplayValue = Mathf.Lerp(currentDisplayValue, targetValue, Time.deltaTime * sensorResponsiveness);

            // Format and Display
            screenText.text = currentDisplayValue.ToString("F2") + " uSv/h";
            if (barGraphText != null)
            {
                // What uSv/h value counts as "100% Full Bar"?
                float maxGraphValue = 5000.0f;

                // Calculate percentage based on the DISPLAYED number
                float percentFull = currentDisplayValue / maxGraphValue;

                // Convert to bars (0 to 20)
                int barCount = (int)Mathf.Clamp(percentFull * 20, 0, 20);

                // Create the string
                string bar = new string('|', barCount);
                string emptySpace = new string(' ', 20 - barCount);

                barGraphText.text = "[" + bar + emptySpace + "]";

                // Force color to black
                barGraphText.color = Color.black;
            }
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