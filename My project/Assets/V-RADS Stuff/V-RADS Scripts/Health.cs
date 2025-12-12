using UnityEngine;
using System.Collections.Generic; // Required for Lists
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    // CHANGED: No longer a single variable. Now a hidden list we fill automatically.
    private List<RadiationHazard> allHazards = new List<RadiationHazard>();

    public float maxDose = 200f;

    [Header("Debug Info")]
    [SerializeField] private float currentDose = 0f;
    [SerializeField] private float currentRadiationLevel = 0f; // See total radiation per second

    [Header("UI & Game Over")]
    public UnityEngine.UI.Slider doseSlider;
    public GameObject gameOverCanvas;
    public LocomotionMediator locomotionMediator;

    private bool isGameOver = false;

    void Start()
    {
        // MAGIC LINE: Find every single object with the "RadiationHazard" script in the scene
        RadiationHazard[] foundHazards = FindObjectsByType<RadiationHazard>(FindObjectsSortMode.None);

        // Add them to our list
        allHazards.AddRange(foundHazards);

        //Debug.Log($"System Online: Found {allHazards.Count} radiation hazards.");
    }

    void Update()
    {
        if (isGameOver) return;

        float totalIntensity = 0f;
        Vector3 headPos = Camera.main.transform.position;

        // LOOP through every hazard
        foreach (RadiationHazard hazard in allHazards)
        {
            if (hazard == null) continue; // Skip if a barrel was deleted

            float dist = Vector3.Distance(headPos, hazard.transform.position);
            dist = Mathf.Max(dist, 0.1f);

            // Add this barrel's radiation to the total
            totalIntensity += hazard.strength / (dist * dist);
        }

        // Save for debugging
        currentRadiationLevel = totalIntensity;

        // Apply Damage
        currentDose += totalIntensity * Time.deltaTime;

        // Update UI
        if (doseSlider != null)
        {
            doseSlider.value = currentDose / maxDose;
        }

        if (currentDose >= maxDose)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            Transform head = Camera.main.transform;
            gameOverCanvas.transform.position = head.position + (head.forward * 1.5f);
            gameOverCanvas.transform.LookAt(head.position);
            gameOverCanvas.transform.Rotate(0, 180, 0);
        }

        if (locomotionMediator != null)
        {
            locomotionMediator.enabled = false;
        }
    }
}