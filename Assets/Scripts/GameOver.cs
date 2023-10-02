using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public TMP_Text gameOverMessage;
    public string[] gameOverMessages;

    public bool resetPrompt = false;
    public KeyCode quitKey = KeyCode.Escape;
    public KeyCode resetKey = KeyCode.R;
    public void GameOverMan() {
        gameOverCanvas.SetActive(true);
        SetRandomMessage(gameOverMessage);
        resetPrompt = true;
    }

    public void RestartButton() {
        SceneManager.LoadScene("Game");
    }

    private void LateUpdate() {
        if (resetPrompt) {
            if (Input.GetKeyDown(quitKey)) {
                ReturnToMenu();
            }

            if (Input.GetKeyDown(resetKey)) {
                RestartButton();
            }
        }
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    private void SetRandomMessage(TMP_Text TM)
    {
        if (gameOverMessages.Length > 0 && gameOverMessages != null)
        {
            int randomIndex = Random.Range(0, gameOverMessages.Length);  // Pick a random index
            TM.SetText(gameOverMessages[randomIndex]);  // Set a random game over message
        }
    }
}
