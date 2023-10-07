using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerAssistant : MonoBehaviour
{
    public GameObject gamePauseStuff;
    public GameObject mapImage;
    public GameObject BlackBG;
    public KeyCode pauseKey = KeyCode.Escape;
    public KeyCode resumeKey = KeyCode.Return;
    public KeyCode restartKey = KeyCode.R;  // Assuming 'R' is for restart
    public KeyCode mapKey = KeyCode.M;
    
    public bool isFaded = false;
    public bool mapOpen = false;

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
            else if (mapOpen) {
                Map();
            } 
            else
            {
                Fade();
                PauseGame();
            }
        }

        if (Input.GetKeyDown(mapKey) && !isPaused) {
            Map();
        }
        if (isPaused && Input.GetKeyDown(resumeKey)) {
            ResumeGame();
        }

        if (isPaused && Input.GetKeyDown(restartKey))
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void Map() {
        if (mapOpen) {
            Fade();
            mapImage.GetComponent<Animator>().SetBool("Open", false);
            mapImage.GetComponent<AudioSource>().Play();
            mapOpen = false;
        } else {
            Fade();
            mapImage.GetComponent<Animator>().SetBool("Open", true);
            mapImage.GetComponent<AudioSource>().Play();
            mapOpen = true;
        }
    }

    public void PauseGame()
    {
        gamePauseStuff.GetComponent<Animator>().SetBool("Paused", true);
        isPaused = true;
        Time.timeScale = 0f;
        backgroundMusic.Pause();  // Pause the background music
    }

    public void Fade() {
        if (isFaded) {
            BlackBG.GetComponent<Animator>().SetBool("Faded", false);
            isFaded = false;
        } else {
            BlackBG.GetComponent<Animator>().SetBool("Faded", true);
            isFaded = true;
        }
    }



    public void ResumeGame()
    {
        gamePauseStuff.GetComponent<Animator>().SetBool("Paused", false);
        isPaused = false;
        Time.timeScale = 1f;
        backgroundMusic.UnPause();  // Resume the background music
        Fade();
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;  // Reset the time scale before leaving to the menu
        backgroundMusic.UnPause();  // Resume the background music before going to the main menu
        SceneManager.LoadScene("MainMenu");
    }
}
