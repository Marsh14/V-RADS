using UnityEngine;

public class DetectRamp : MonoBehaviour
{
    // Type the exact name of your ramp object here in the Inspector
    public string targetObjectName = "InvisibleRamp";

    // Timer to prevent console flooding
    private float lastLogTime = 0f;

    // This special function runs only when a CharacterController hits something
    // Use this INSTEAD of OnCollisionEnter
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // This detects when your capsule hits something while moving
        //Debug.Log("I hit: " + hit.gameObject.name);
    }
}