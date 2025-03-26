using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead { get; private set; }

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private CanvasGroup gameOverScreen;
    [SerializeField] private AudioSource gameOverAudioSource;  
    [SerializeField] private AudioClip gameOverMusic;

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthSlider();

        gameOverScreen.alpha = 0;
        gameOverScreen.gameObject.SetActive(false);

        Time.timeScale = 1;

        GetComponent<Animator>().Play("Idle");
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

        ScreenShakeManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);

            PlayGameOverMusic(); 

            StartCoroutine(DeathSequence());
        }
    }

    private void PlayGameOverMusic()
    {
        BackgroundMusicController bgm = Object.FindAnyObjectByType<BackgroundMusicController>();
        if (bgm != null && bgm.audioSource.isPlaying)
        {
            bgm.audioSource.Stop();
        }

        if (gameOverAudioSource != null && gameOverMusic != null)
        {
            gameOverAudioSource.clip = gameOverMusic;
            gameOverAudioSource.loop = false;
            gameOverAudioSource.Play();
        }
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FadeToGameOver());

        yield return new WaitForSeconds(10f);
        PlayGameOverMusic();
    }

    private IEnumerator FadeToGameOver()
    {
        gameOverScreen.gameObject.SetActive(true);

        float fadeDuration = 2f;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            gameOverScreen.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            yield return null;
        }

        gameOverScreen.alpha = 1;
        Time.timeScale = 0;
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        UpdateHealthSlider();

        if (currentHealth <= 0)
        {
            CheckIfPlayerDeath();
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }
}

