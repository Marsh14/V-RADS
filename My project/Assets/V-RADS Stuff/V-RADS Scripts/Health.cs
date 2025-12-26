using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

// This script manages the player's health (max dose of radiation) based on radiation exposure
public class Health : MonoBehaviour
{
    [Header("Connections")]
    public GameManager gameManager; 

    [Header("Health Settings")]
    private List<RadiationHazard> allHazards = new List<RadiationHazard>();
    public float maxDose = 200f;

    [Header("Debug Info")]
    [SerializeField] private float currentDose = 0f;

    [Header("UI")]
    public UnityEngine.UI.Slider doseSlider;
    public GameObject gameOverCanvas;
    private bool isGameOver = false;

    void Start()
    {
        // Find all RadiationHazard objects in the scene
        RadiationHazard[] foundHazards = FindObjectsByType<RadiationHazard>(FindObjectsSortMode.None);
        allHazards.AddRange(foundHazards);
    }

    void Update()
    {
        if (isGameOver) return;

        
        float totalIntensity = 0f;
        Vector3 headPos = Camera.main.transform.position;
        // Use the same formula for the geiger counter to calculate total radiation intensity at the player's head position
        foreach (RadiationHazard hazard in allHazards)
        {
            if (hazard == null) continue;
            float dist = Vector3.Distance(headPos, hazard.transform.position);
            dist = Mathf.Max(dist, 0.1f);
            totalIntensity += hazard.strength / (dist * dist);
        }

        // Increase current dose based on total intensity and time
        currentDose += totalIntensity * Time.deltaTime;

        if (doseSlider != null) doseSlider.value = currentDose / maxDose;

        // If health is depleted, trigger game over
        if (currentDose >= maxDose)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        // Show the UI 
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            Transform head = Camera.main.transform;
            gameOverCanvas.transform.position = head.position + (head.forward * 1.5f);
            gameOverCanvas.transform.LookAt(head.position);
            gameOverCanvas.transform.Rotate(0, 180, 0);
        }

        // CALL THE OTHER SCRIPT
        if (gameManager != null)
        {
            gameManager.EndGame(); 
        }
    }
}