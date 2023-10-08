using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject logoScreenCover;
    public GameObject MainMenuObj;
    public GameObject pressStart;
    private bool CoverStartActive = true;
    private bool MainMenuActive = false;
    public KeyCode StartKey = KeyCode.Return;
    public KeyCode EscapeKey = KeyCode.Escape;
    private void Update() {
        if(CoverStartActive && Input.GetKeyDown(StartKey) && !MainMenuActive) {
            logoScreenCover.GetComponent<Animator>().SetBool("CoverIsOpen", true);
            pressStart.GetComponent<Animator>().SetBool("MenuIsOpen", true);
            MainMenuObj.GetComponent<Animator>().SetBool("MenuIsOpen", true);
            CoverStartActive = false;
            MainMenuActive = true;
        }
        
        if(!CoverStartActive && Input.GetKeyDown(EscapeKey) && MainMenuActive) {
            logoScreenCover.GetComponent<Animator>().SetBool("CoverIsOpen", false);
            pressStart.GetComponent<Animator>().SetBool("MenuIsOpen", false);
            MainMenuObj.GetComponent<Animator>().SetBool("MenuIsOpen", false);
            CoverStartActive = true;
            MainMenuActive = false;
        }

        if(CoverStartActive && Input.GetKeyDown(EscapeKey)) {
            ExitGame();
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void OpenOptionMenu() {
        //
    }
    
}
