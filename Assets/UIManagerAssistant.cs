using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerAssistant : MonoBehaviour
{
    public GameObject gamePauseCanvas;
    public KeyCode pauseKey = KeyCode.Escape;
    public KeyCode resumeKey = KeyCode.Return;
    public KeyCode restartKey = KeyCode.R;  // Assuming 'R' is for restart

    private bool isPaused = false;
    public AudioSource backgroundMusic;  // Reference to the AudioSource component

    private void LateUpdate()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
            {
                ReturnToMenu();
            }
            else
            {
                PauseGame();
            }
        }

        if (isPaused && Input.GetKeyDown(resumeKey)) {
            ResumeGame();
        }

        if (isPaused && Input.GetKeyDown(restartKey))
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void PauseGame()
    {
        gamePauseCanvas.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
        backgroundMusic.Pause();  // Pause the background music
    }

    public void ResumeGame()
    {
        gamePauseCanvas.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        backgroundMusic.UnPause();  // Resume the background music
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;  // Reset the time scale before leaving to the menu
        backgroundMusic.UnPause();  // Resume the background music before going to the main menu
        SceneManager.LoadScene("MainMenu");
    }
}
