using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth PlayerH;
    public GameObject player;
    public Image[] heartContainers;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public int playerHealth = 6;  // Assuming the player's health ranges from 0 to 6
    public int maxPlayerHealth = 6;

    [SerializeField]
    private float invincibilityDurationSeconds = 1.5f;
    private bool isInvincible = false;
    [SerializeField] private SpriteRenderer spriteBodyRenderer, spriteHeadRenderer;

    private void Awake() {
        PlayerH = this;
    }

    public AudioSource playerHurtSource;
    public AudioClip[] playerHurtClips;

    // You could initialize playerHealth somewhere, 
    // for example in a Start method or from another script

    void Start() {
        UpdateHearts();
    }
    public void UpdateHearts()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            int healthForThisContainer = playerHealth - i * 2;

            if (healthForThisContainer >= 2)
            {
                heartContainers[i].sprite = fullHeart;
            }
            else if (healthForThisContainer == 1)
            {
                heartContainers[i].sprite = halfHeart;
            }
            else
            {
                heartContainers[i].sprite = emptyHeart;
            }
        }
    }

    public void SubtractPlayerHealth(int adj) {

        if (isInvincible) return;

        playerHealth -= adj;
        if (playerHealth < 0) {
            playerHealth = 0;
        }

        if (playerHealth <= 0) {
            // Gameover called here
            this.GetComponent<GameOver>().GameOverMan();
            player.SetActive(false);
        }

        UpdateHearts();
        PlayRandomHurtSound(playerHurtSource);
        StartCoroutine(BecomeTemporarilyInvincible());
    }

    public void AddPlayerHealth(int adj) {
        playerHealth += adj;
        if (playerHealth > maxPlayerHealth) {
            playerHealth = maxPlayerHealth;
        }

        UpdateHearts();
    }

    private IEnumerator BecomeTemporarilyInvincible()
{
    Debug.Log("Player turned invincible!");
    isInvincible = true;

    StartCoroutine(FlickerSprite());

    float elapsedTime = 0f;
    while (elapsedTime < invincibilityDurationSeconds)
    {
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    Debug.Log("Player is no longer invincible!");
    isInvincible = false;

    StopCoroutine(FlickerSprite());
}



    void PlayRandomHurtSound(AudioSource PlayerSource)
    {
        if (playerHurtClips.Length > 0 && PlayerSource != null)
        {
            int randomIndex = Random.Range(0, playerHurtClips.Length);  // Pick a random index
            PlayerSource.PlayOneShot(playerHurtClips[randomIndex]);  // Play the corresponding audio clip
        }
    }

    private IEnumerator FlickerSprite() {
        while (isInvincible) {
            spriteHeadRenderer.enabled = !spriteHeadRenderer.enabled;
            spriteBodyRenderer.enabled = !spriteBodyRenderer.enabled;
            yield return new WaitForSeconds(0.1f); // This is the time it flickers
        }

        spriteBodyRenderer.enabled = true;
        spriteHeadRenderer.enabled = true;
    }

}
