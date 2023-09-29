using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject logoScreenCover;
    private bool CoverStartActive = true;
    public KeyCode StartKey = KeyCode.Return;
    private void Update() {
        if(CoverStartActive && Input.GetKeyDown(StartKey)) {
            logoScreenCover.SetActive(false);
        }
    }
    
}
