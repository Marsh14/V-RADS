using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Locomotion Components")]
    // Drag your "Continuous Move" object here in the Inspector
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement.ContinuousMoveProvider moveProvider;

    // Optional: Drag "Snap Turn" here if you want to stop them from turning too
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning.SnapTurnProvider turnProvider;

    public string gameSceneName = "Title";

    public void EndGame()
    {
        // 1. Disable Joystick Movement
        if (moveProvider != null)
        {
            moveProvider.enabled = false;
        }

        // 2. (Optional) Disable Joystick Turning
        // If you want them to ONLY look with their real head, disable this too.
        if (turnProvider != null)
        {
            turnProvider.enabled = false;
        }

        Debug.Log("Player Movement Frozen.");
    }
    public void RestartGame()
    {
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MenuGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
