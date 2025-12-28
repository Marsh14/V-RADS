using UnityEngine;

public class PlayerFootstepsPosition : MonoBehaviour
{
    [Header("References")]
    public AudioSource footstepSource;

    [Header("Settings")]
    public float walkThreshold = 0.1f; // Minimum speed to trigger sound

    private Vector3 lastPosition;
    private bool isMoving;

    void Start()
    {
        // Initialize position
        lastPosition = transform.position;
    }

    void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        float currentSpeed = distanceMoved / Time.deltaTime;

        // Save position for the next frame
        lastPosition = transform.position;

        // Debug.Log("Real Speed: " + currentSpeed);

        // Play sound if moving above threshold, if not, pause it
        if (currentSpeed > walkThreshold)
        {
            if (!footstepSource.isPlaying)
            {
                footstepSource.Play();
            }
        }
        else
        {
            if (footstepSource.isPlaying)
            {
                footstepSource.Pause();
            }
        }
    }
}