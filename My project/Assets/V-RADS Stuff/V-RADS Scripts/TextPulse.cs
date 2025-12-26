using UnityEngine;
using TMPro;

// This script makes the title text object pulse in size.
public class TextPulse : MonoBehaviour
{
    public float speed = 2f;
    public float minSize = 0.9f;
    public float maxSize = 1.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the scale based on a sine wave (Lerp between minSize and maxSize)
        float scale = Mathf.Lerp(minSize, maxSize, (Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
