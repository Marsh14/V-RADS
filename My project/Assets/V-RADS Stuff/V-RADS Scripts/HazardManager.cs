using UnityEngine;
using System.Collections.Generic;

public class HazardManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How many items should be dangerous this round?")]
    public int numberOfHazardsToActivate = 7;
    public GameObject successScreen;

    private int hazardsFound = 0;
    private float timer = 0f;
    private bool isGameActive = true;
    public TMPro.TMP_Text timeDisplayText; 

    [Header("References")]
    // List of all possible hazards in the scene
    public List<RadiationHazard> allPotentialHazards;

    void Start()
    {
        if (successScreen != null) successScreen.SetActive(false);
        RandomizeHazards();

        // Reset timer just in case
        timer = 0f;
        isGameActive = true;
    }

    void Update()
    {
        // Only count up if the game is still going
        if (isGameActive)
        {
            timer += Time.deltaTime;
        }
    }

    public void RandomizeHazards()
    {
        // Turn OFF everything first (Safety First)
        foreach (var hazard in allPotentialHazards)
        {
            hazard.SetActiveHazard(false);
        }

        // Shuffle the list (Fisher-Yates Shuffle Algorithm)
        // This ensures a truly random selection of hazards (and only O(n) complexity)
        // This mixes up the order of the barrels in our list randomly
        for (int i = 0; i < allPotentialHazards.Count; i++)
        {
            RadiationHazard temp = allPotentialHazards[i];
            int randomIndex = Random.Range(i, allPotentialHazards.Count);
            allPotentialHazards[i] = allPotentialHazards[randomIndex];
            allPotentialHazards[randomIndex] = temp;
        }

        // Activate the first X items in the shuffled list
        // We limit the count so we don't try to activate more hazards than exist
        int count = Mathf.Min(numberOfHazardsToActivate, allPotentialHazards.Count);

        for (int i = 0; i < count; i++)
        {
            allPotentialHazards[i].SetActiveHazard(true);
            Debug.Log($"Hazard Activated: {allPotentialHazards[i].name}");
        }
    }
    public void RegisterFixedHazard()
    {
        hazardsFound++;
        //Debug.Log("Hazards Cleaned: " + hazardsFound + " / " + numberOfHazardsToActivate);

        if (hazardsFound >= numberOfHazardsToActivate)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        isGameActive = false;

        // Format the time nicely (Minutes : Seconds)
        // Mathf.FloorToInt chops off the decimals
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        // Destroy any existing popups
        foreach (var hazard in allPotentialHazards)
        {
            if (hazard != null)
            {
                hazard.DestroyPopup();
            }
        }

        // Update the UI Text
        if (timeDisplayText != null)
        {
           
           timeDisplayText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }

        // Show the screen
        if (successScreen != null)
        {
            successScreen.SetActive(true);
            Transform head = Camera.main.transform;
            successScreen.transform.position = head.position + (head.forward * 1.5f);
            successScreen.transform.LookAt(head.position);
            successScreen.transform.Rotate(0, 180, 0);
        }
    }

}
