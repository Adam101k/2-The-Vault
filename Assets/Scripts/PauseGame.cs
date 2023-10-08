using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseGame : MonoBehaviour
{
    public GameObject gamePauseCanvas;

    public bool escapePrompt = false;
    public KeyCode quitKey = KeyCode.Escape;
    public void TriggerPauseGame() {
        gamePauseCanvas.SetActive(true);
        escapePrompt = true;
    }

    public void TriggerResumeGame() {
        gamePauseCanvas.SetActive(false);
        escapePrompt = false;
    }

    public void RestartButton() {
        SceneManager.LoadScene("Game");
    }

    private void LateUpdate() {
        if (escapePrompt && Input.GetKeyDown(quitKey)) {
            ReturnToMenu();
        }
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
