using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuButtons : MonoBehaviour
{
    public string gameSceneName = "V-RADS";
    public TMP_Text buttonText;
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
}
