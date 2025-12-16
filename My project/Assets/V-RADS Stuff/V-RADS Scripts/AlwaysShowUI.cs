using UnityEngine;
using UnityEngine.UI; // Needed for UI components

public class AlwaysShowUI: MonoBehaviour
{
    void Start()
    {
        // 1. Try to find a standard Image (Panels, Buttons)
        Image img = GetComponent<Image>();

        // 2. If not found, try to find a RawImage (Your Radiation Picture)
        RawImage rawImg = GetComponent<RawImage>();

        // 3. Apply the fix to whichever one we found
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