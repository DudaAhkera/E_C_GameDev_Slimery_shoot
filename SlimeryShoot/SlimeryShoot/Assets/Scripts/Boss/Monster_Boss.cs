using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Monster_Boss : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip deathSound;

    [Header("Configuracoes do Boss")]
    public float maxHealth = 10f; // Aguenta 10 tiros
    private float currentHealth;

    public GameObject laserPrefab;
    public Transform laserSpawn;
    public float fireRate = 2f;
    public float moveSpeed = 2f;        // Velocidade de avanco

    private Transform player;            // Referencia ao player
    private float nextFireTime = 0f;
    private SpriteRenderer sprite;
    private Color baseColor;
    private Rigidbody2D rb;
    private bool isDead = false;

    [Header("Reacao ao Dano")]
    public float knockbackForce = 2f;           // Forca do empurrao ao levar dano
    public float flashDuration = 0.1f;          // Tempo do flash de dano
    public Color flashColor = Color.green;        // Cor que pisca ao ser atingido

    void Start()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        baseColor = sprite.color;

        // Caso o rigidbody seja usado apenas para impacto, garantir que nao caia
        rb.gravityScale = 0;
        rb.freezeRotation = true;
            // Busca automatica do Player pela tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Atualiza HUD no inicio
        if (GameHUDManager.Instance != null)
            GameHUDManager.Instance.SetBossHealth(currentHealth, maxHealth);

    }

    void Update()
    {
        if (isDead) return;

        // Movimento em direcao ao player
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }

        // Faz o boss atacar periodicamente
        if (Time.time >= nextFireTime)
        {
            FireLaser();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireLaser()
    {
        if (laserPrefab == null || laserSpawn == null || player == null)
            return;

        // So atira se o player estiver a esquerda
        if (player.position.x < transform.position.x)
        {
            GameObject laser = Instantiate(laserPrefab, laserSpawn.position, laserSpawn.rotation);

            // Ajusta direcao do laser para o player
            Vector2 dir = (player.position - laserSpawn.position).normalized;
            laser.GetComponent<SlimeProjectiles>().direction = dir;
        }
    }

    public void TakeHit(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Boss levou {amount} de dano! Vida restante: {currentHealth}");

        StartCoroutine(FlashDamage());

        // Aplica empurrao leve para reagir visualmente
        Vector2 randomKnockback = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
        rb.AddForce(randomKnockback, ForceMode2D.Impulse);

        // Atualiza HUD
        if (GameHUDManager.Instance != null)
            GameHUDManager.Instance.SetBossHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();

    }

    // Efeito visual de dano (pisca rapidamente)
    private IEnumerator FlashDamage()
    {
        sprite.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        sprite.color = baseColor;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Boss derrotado!");

        // --- PARA TUDO ---
        rb.linearVelocity = Vector2.zero;
        rb.freezeRotation = false;  // permitir girar
        this.enabled = false;       // parar Update()

        // --- TOCA O SOM DE MORTE ---
        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 2f);

        // --- FLASH FINAL ---
        sprite.color = Color.magenta;

        // --- ANIMACAO: DEITAR ---
        StartCoroutine(DeathAnimation());

    }

    private IEnumerator DeathAnimation()
    {
        // gira em 90 graus suave
        Quaternion targetRot = Quaternion.Euler(0, 0, -90f);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 4f;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, t);
            yield return null;
        }

        // espera 1 segundo antes de sumir
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);

        FindObjectOfType<ProgressTracker>().RegisterBossKill();
    }


}

