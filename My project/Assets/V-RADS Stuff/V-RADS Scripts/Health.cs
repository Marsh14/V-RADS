using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class Health : MonoBehaviour
{
    [Header("Connections")]
    public GameManager gameManager; // <--- ADD THIS! 

    [Header("Health Settings")]
    private List<RadiationHazard> allHazards = new List<RadiationHazard>();
    public float maxDose = 200f;

    [Header("Debug Info")]
    [SerializeField] private float currentDose = 0f;

    [Header("UI")]
    public UnityEngine.UI.Slider doseSlider;
    public GameObject gameOverCanvas;
    // REMOVED: public LocomotionMediator locomotionMediator; (GameManager handles this now)

    private bool isGameOver = false;

    void Start()
    {
        RadiationHazard[] foundHazards = FindObjectsByType<RadiationHazard>(FindObjectsSortMode.None);
        allHazards.AddRange(foundHazards);
    }

    void Update()
    {
        if (isGameOver) return;

        // ... (Your existing radiation calculation code stays exactly the same) ...
        float totalIntensity = 0f;
        Vector3 headPos = Camera.main.transform.position;
        foreach (RadiationHazard hazard in allHazards)
        {
            if (hazard == null) continue;
            float dist = Vector3.Distance(headPos, hazard.transform.position);
            dist = Mathf.Max(dist, 0.1f);
            totalIntensity += hazard.strength / (dist * dist);
        }

        currentDose += totalIntensity * Time.deltaTime;

        if (doseSlider != null) doseSlider.value = currentDose / maxDose;

        if (currentDose >= maxDose)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        // 1. Show the UI 
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            Transform head = Camera.main.transform;
            gameOverCanvas.transform.position = head.position + (head.forward * 1.5f);
            gameOverCanvas.transform.LookAt(head.position);
            gameOverCanvas.transform.Rotate(0, 180, 0);
        }

        // 2. CALL THE OTHER SCRIPT
        if (gameManager != null)
        {
            gameManager.EndGame(); // <--- This freezes the player!
        }
    }
}