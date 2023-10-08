using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public KeyCode EscapeKey = KeyCode.Escape;

    private void Update() {
        if (Input.GetKeyDown(EscapeKey)) {
            SceneManager.LoadScene(0);
        }
    }
}
