using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class SlimeProjectiles : MonoBehaviour
{
    [Header("Configuracoes do Projetil")]
    public float speed = 10f;
    public float lifeTime = 2f;
    public Vector2 direction = Vector2.right;

    [Header("Configuracoes de Colisao")]
    public string[] targetTags;       // Lista de tags que o projétil pode atingir
    public bool destroyOnHit = true;
    public int pointsOnHit = 0; // pontos para HUD (opcional)
    public float damage = 1f;

    [Header("Som de Impacto")]
    public AudioClip impactSound;
    [Range(0f, 1f)] 
    public float impactVolume = 1f;

    [Header("Visuais")]
    public Sprite customSprite;
    public Color projectileColor = Color.white;

    [Header("Balanco")]
    public bool enableTilt = false;
    public float tiltAmount = 10f;
    public float tiltSpeed = 3f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float tiltTimer = 0f;
    private AudioSource audioSource;

    public AudioClip projectileSound;

    void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

    }

    void Start()
    {
        // Configuracao visual
        if (customSprite != null)
            spriteRenderer.sprite = customSprite;

        spriteRenderer.color = projectileColor;

        // Fisica basica
        rb.isKinematic = true; // projétil não precisa de física real
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // evita passar pelos inimigos

        // Som de saida (laser)
        if (projectileSound != null && audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.clip = projectileSound;
            audioSource.Play();
        }

        // autodestruição
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction.normalized * speed * Time.fixedDeltaTime);
    }

    void Update()
    {

        if (enableTilt)
        {
            tiltTimer += Time.deltaTime * tiltSpeed;
            float zRotation = Mathf.Sin(tiltTimer) * tiltAmount;
            transform.rotation = Quaternion.Euler(0, 0, zRotation);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetTags == null || targetTags.Length == 0)
            return;

        foreach (string tag in targetTags)
        {
            if (!collision.CompareTag(tag))
                continue;


            Debug.Log($"{gameObject.name} atingiu {collision.gameObject.name} com tag {tag}");

            Debug.Log("Projetil encostou em: " + collision.tag);

            Debug.Log("Laser colidiu com: " + collision.name + " TAG: " + collision.tag);


            switch (tag)
            {
                case "Enemy": // SlimeTrashMonster

                    // 🔊 Som de impacto SOMENTE para Slime Trash Monster
                    if (impactSound != null)
                        AudioSource.PlayClipAtPoint(impactSound, transform.position, impactVolume);

                    var slime = collision.GetComponent<SlimeTrash_Monster>();
                    if (slime != null)
                        slime.TakeHit();
                    break;

                case "BossEnemy": // Boss
                    var boss = collision.GetComponent<Monster_Boss>();
                    if (boss != null)
                        boss.TakeHit(damage);
                    break;

                case "Player":
                    var player = collision.GetComponent<PlayerHealth>(); // ou seu script de vida do player
                    if (player != null)
                        player.TakeDamage(damage);
                    break;

                default:
                    Debug.Log($"Tag {tag} atingida, mas sem comportamento definido.");
                    break;

            }

            if (destroyOnHit)
                Destroy(gameObject);

            return; // Sai do foreach
        }
    }

}



