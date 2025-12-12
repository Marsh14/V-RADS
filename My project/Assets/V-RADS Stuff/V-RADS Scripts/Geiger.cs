using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GeigerCounter : MonoBehaviour
{
    [Header("Setup")]
    public Transform radiationSource;
    public AudioSource audioSource;

    [Header("Physics Settings")]
    [Tooltip("How strong is the radiation? (Try 10 to start)")]
    public float sourceStrength = 10.0f;

    // We clamp the distance so we don't divide by zero!
    private float minDistance = 0.1f;

    [Header("Geiger Feedback")]
    public float maxClickDelay = 1.0f; // Slow clicking (Safe)
    public float minClickDelay = 0.05f; // Fast buzzing (Danger)

    private float nextClickTime = 0f;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void Update()
    {
        if (radiationSource == null) return;

        // 1. CALCULATE DISTANCE (With Physics Clamp)
        float distance = Vector3.Distance(transform.position, radiationSource.position);
        distance = Mathf.Max(distance, minDistance);

        // 2. INVERSE SQUARE LAW
        float intensity = sourceStrength / (distance * distance);

        // 3. CALCULATE DELAY
        // We calculate the base speed
        float targetDelay = Mathf.Clamp(1.0f / intensity, minClickDelay, maxClickDelay);

        // 4. TRIGGER CLICK
        if (Time.time >= nextClickTime)
        {
            PlayClick(intensity);

            // REALISM TRICK:
            // Add a tiny random variation to the time so it's not a perfect metronome.
            // Random.Range(0, 0.1f) makes it hesitate slightly sometimes.
            float randomFactor = Random.Range(0f, targetDelay * 0.2f);

            nextClickTime = Time.time + targetDelay + randomFactor;
        }
    }

    void PlayClick(float intensity)
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(audioSource.clip);

        // Map intensity to haptic strength (0.0 to 1.0) for the controller
        // We clamp it because intensity can go really high in the Inverse Square law
        float hapticStrength = Mathf.Clamp01(intensity / 10.0f);

        TriggerHaptics(hapticStrength);
    }

    void TriggerHaptics(float strength)
    {
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            var hand = grabInteractable.interactorsSelecting[0];
            if (hand is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor controllerInteractor)
            {
                controllerInteractor.SendHapticImpulse(strength, 0.1f);
            }
        }
    }
}