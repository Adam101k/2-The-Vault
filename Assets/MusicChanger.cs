using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    public GameObject levelMusicPlayer;
    public GameObject roomMusicPlayer;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            levelMusicPlayer.GetComponent<AudioSource>().Pause();
            roomMusicPlayer.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            levelMusicPlayer.GetComponent<AudioSource>().Play();
            roomMusicPlayer.GetComponent<AudioSource>().Pause();
        }
    }
}
