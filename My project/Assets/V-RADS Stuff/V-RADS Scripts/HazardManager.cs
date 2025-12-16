using UnityEngine;
using System.Collections.Generic;

public class HazardManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How many items should be dangerous this round?")]
    public int numberOfHazardsToActivate = 3;

    [Header("References")]
    // Drag EVERY potential hazard into this list in the Inspector
    public List<RadiationHazard> allPotentialHazards;

    void Start()
    {
        RandomizeHazards();
    }

    public void RandomizeHazards()
    {
        // 1. Turn OFF everything first (Safety First)
        foreach (var hazard in allPotentialHazards)
        {
            hazard.SetActiveHazard(false);
        }

        // 2. Shuffle the list (Fisher-Yates Shuffle Algorithm)
        // This ensures a truly random selection of hazards (and only O(n) complexity)
        // This mixes up the order of the barrels in our list randomly
        for (int i = 0; i < allPotentialHazards.Count; i++)
        {
            RadiationHazard temp = allPotentialHazards[i];
            int randomIndex = Random.Range(i, allPotentialHazards.Count);
            allPotentialHazards[i] = allPotentialHazards[randomIndex];
            allPotentialHazards[randomIndex] = temp;
        }

        // 3. Activate the first X items in the shuffled list
        // We limit the count so we don't try to activate more hazards than exist
        int count = Mathf.Min(numberOfHazardsToActivate, allPotentialHazards.Count);

        for (int i = 0; i < count; i++)
        {
            allPotentialHazards[i].SetActiveHazard(true);
            Debug.Log($"Hazard Activated: {allPotentialHazards[i].name}");
        }
    }
}