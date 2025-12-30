using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// This script handles the menu button actions
public class MenuButtons : MonoBehaviour
{
    public string gameSceneName = "V-RADS";
    public TMP_Text buttonText; // For start
    public TMP_Text hazardCountText;
    public TMP_Text healthText;
    

    void Start()
    {
        UpdateUI();
    }
    public void StartSimulation()
    {
        if (buttonText != null)
        {
            buttonText.text = "Loading...";
        }
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        //Debug.Log("Quitting Application...");
        Application.Quit();
    }

    // --- HAZARD BUTTONS ---
    public void IncreaseHazards()
    {
        GameSettings.HazardCount++;
        // Cap amount of active hazards to 15
        if (GameSettings.HazardCount > 10) GameSettings.HazardCount = 10;
        UpdateUI();
    }

    public void DecreaseHazards()
    {
        GameSettings.HazardCount--;
        if (GameSettings.HazardCount < 1) GameSettings.HazardCount = 1;
        UpdateUI();
    }

    // --- HEALTH BUTTONS ---
    public void IncreaseHealth()
    {
        GameSettings.MaxHealth += 10; // Go up by 10s
        if (GameSettings.MaxHealth > 500) GameSettings.MaxHealth = 500; // Cap at 500
        UpdateUI();
    }

    public void DecreaseHealth()
    {
        GameSettings.MaxHealth -= 10;
        if (GameSettings.MaxHealth < 10) GameSettings.MaxHealth = 10;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (hazardCountText != null) hazardCountText.text = GameSettings.HazardCount.ToString();
        if (healthText != null) healthText.text = GameSettings.MaxHealth.ToString();
    }
}
