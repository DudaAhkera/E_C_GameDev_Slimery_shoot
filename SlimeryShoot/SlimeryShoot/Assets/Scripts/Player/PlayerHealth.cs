using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuracao de Vida")]
    public float maxHealth = 100f;
    private float currentHealth;

    private Animator anim;
    private bool isDead = false;

    [Header("Regeneracao de Vida")]
    public float regenAmount = 5f;      // quanto de vida regenera por tick
    public float regenInterval = 1f;    // quanto tempo entre regenerações
    public float regenDelay = 3f;       // quanto tempo esperar depois de levar dano

    private bool isRegenerating = false;

    [Header("Efeitos Visuais de Regeneracao")]
    public Color regenGlowColor = new Color(1f, 0.8f, 0.3f); // dourado/brilhante
    public float glowIntensity = 2f;
    public float glowSpeed = 5f;

    private SpriteRenderer sprite;
    private Color originalColor;
    private bool glowing = false;


    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        if (sprite != null)
            originalColor = sprite.color;

        if (GameHUDManager.Instance != null)
            GameHUDManager.Instance.SetPlayerHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (GameHUDManager.Instance != null)
            GameHUDManager.Instance.SetPlayerHealth(currentHealth, maxHealth);
        


        // Reinicia a regeneração depois de levar dano
        StartRegen();

        if (!isDead)
            StartRegen();

        if (currentHealth <= 0)
            Die();
    }

    // REGENERACAO

    void StartRegen()
    {
        if (isRegenerating)
            StopCoroutine("RegenCoroutine");

        StartCoroutine("RegenCoroutine");
    }

    private IEnumerator RegenCoroutine()
    {
        isRegenerating = true;

        // Espera antes de começar a regenerar
        yield return new WaitForSeconds(regenDelay);

        // LIGA efeitos visuais
        StartGlowEffect();
        if (GameHUDManager.Instance != null)
            GameHUDManager.Instance.StartHealthBlink();

        while (currentHealth < maxHealth && !isDead)
        {
            currentHealth += regenAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (GameHUDManager.Instance != null)
                GameHUDManager.Instance.SetPlayerHealth(currentHealth, maxHealth);

            yield return new WaitForSeconds(regenInterval);
        }

        // DESLIGA efeitos
        StopGlowEffect();
        if (GameHUDManager.Instance != null)
            GameHUDManager.Instance.StopHealthBlink();

        isRegenerating = false;
    }


    // GLOW EFFECT 

    void StartGlowEffect()
    {
        if (sprite == null) return;

        glowing = true;
        StartCoroutine(GlowPulse());
    }

    void StopGlowEffect()
    {
        glowing = false;
        if (sprite != null)
            sprite.color = originalColor;
    }

    private IEnumerator GlowPulse()
    {
        while (glowing)
        {
            if (sprite == null) yield break;

            float t = (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f; 

            sprite.color = Color.Lerp(originalColor, regenGlowColor * glowIntensity, t);

            yield return null;
        }
    }

    // MORTE 
    private void Die()
    {
        if (isDead) return;

        isDead = true;

        // animação
        if (anim != null)
            anim.SetTrigger("Die");

        // salva score de forma segura
        int score = 0;
        if (GameHUDManager.Instance != null)
            score = GameHUDManager.Instance.CurrentScore;

        PlayerPrefs.SetInt("LastScore", score);

        int best = PlayerPrefs.GetInt("BestScore", 0);
        if (score > best)
            PlayerPrefs.SetInt("BestScore", score);

        // salva última fase jogada
        MenuController.lastPlayedScene = SceneManager.GetActiveScene().name;

        // troca de cena SEM atualizar HUD depois disso
        SceneManager.LoadScene("GameOver");
    }
}



