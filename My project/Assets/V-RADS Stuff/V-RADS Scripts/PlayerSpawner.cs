using UnityEngine;
using System.Collections;

// This script moves the player to a specified spawn point at the start of the game
public class PlayerSpawner : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Transform spawnPoint; 

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (spawnPoint != null)
        {
            StartCoroutine(MovePlayerRoutine());
        }
    }

    IEnumerator MovePlayerRoutine()
    {
        // Wait for the end of the frame. 
        // This lets Unity finish loading the XR Rig and Input systems.
        yield return new WaitForEndOfFrame();

        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Now move
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        if (characterController != null)
        {
            characterController.enabled = true;
        }
    }
}