using UnityEngine;
using UnityEngine.SceneManagement;

// This script manages game state and has functions for the buttons
public class GameManager : MonoBehaviour
{
    [Header("Locomotion Components")]
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement.ContinuousMoveProvider moveProvider;
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning.SnapTurnProvider turnProvider;

    public string gameSceneName = "Title";

    public void EndGame()
    {
        // Disable Joystick Movement
        if (moveProvider != null)
        {
            moveProvider.enabled = false;
        }

        // Disable Joystick Turning
        if (turnProvider != null)
        {
            turnProvider.enabled = false;
        }

        //Debug.Log("Player Movement Frozen.");
    }
    public void RestartGame()
    {
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MenuGame()
    {
        // Loads the title scene
        SceneManager.LoadScene(gameSceneName);
    }
}
