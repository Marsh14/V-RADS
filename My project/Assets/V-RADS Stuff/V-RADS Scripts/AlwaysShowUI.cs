using UnityEngine;
using UnityEngine.UI; 

public class AlwaysShowUI: MonoBehaviour
{
    // This script ensures that the UI element this is attached to always renders on top of other objects.
    void Start()
    {
        Image img = GetComponent<Image>();
        RawImage rawImg = GetComponent<RawImage>();

        if (img != null)
        {
            img.material = CreateAlwaysOnTopMaterial(img.material);
        }
        else if (rawImg != null)
        {
            rawImg.material = CreateAlwaysOnTopMaterial(rawImg.material);
        }
    }

    // Helper function to clone the material and turn off Z-Test
    Material CreateAlwaysOnTopMaterial(Material original)
    {
        Material newMat = new Material(original);
        // 8 = Always draw on top (ignore walls/objects)
        newMat.SetInt("unity_GUIZTestMode", 8);
        return newMat;
    }
}