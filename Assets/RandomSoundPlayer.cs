using System.Collections;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    private float minInterval = 3f;
    private float maxInterval = 15f;
    private float nextSoundTime;

    void Start()
{
    nextSoundTime = Time.time + Random.Range(minInterval, maxInterval);
}


void Update()
{
    if (Time.time >= nextSoundTime)
    {
        PlayRandomSound();
        nextSoundTime = Time.time + Random.Range(minInterval, maxInterval);
    }
}

void PlayRandomSound()
{
    if (audioClips.Length > 0)
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[randomIndex];
        audioSource.Play();
    }
}

}
