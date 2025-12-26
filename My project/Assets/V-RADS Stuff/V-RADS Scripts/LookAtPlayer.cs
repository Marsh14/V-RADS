using UnityEngine;

// This script makes the attached UI element always face the player's headset
public class LookAtPlayer : MonoBehaviour
{
    void Update()
    {
        if (Camera.main != null)
        {
            // Rotate the UI to face the Main Camera (Headset)
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
        }
    }
}