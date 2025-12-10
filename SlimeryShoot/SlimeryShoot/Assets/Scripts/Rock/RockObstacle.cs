using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class RockObstacle : MonoBehaviour
{
    [Header("Configuracoes de movimento")]
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    public float angularSpeed = 200f;   // rotacao para efeito de "rolar"

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    [Header("Impacto no Player")]
    public float playerSlowAmount = 0.5f; // reduz a velocidade do player ao bater
    public float knockbackForce = 5f;     // forca do empurrao ao bater
    public float slowDuration = 1f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = false;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Escolhe uma direcao aleatoria 2D
        float randomY = Random.Range(-0.3f, 0.3f);
        moveDirection = new Vector2(-1f, randomY).normalized;
        rb.linearVelocity = moveDirection * Random.Range(minSpeed, maxSpeed);
        rb.angularVelocity = Random.Range(-angularSpeed, angularSpeed);

        //AUTO-DESTRUIÇÃO
        Destroy(gameObject, 8f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se colidir com player
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerMoviment>(); // ou o seu script de controle do player
            if (player != null)
            {
                player.AdjustSpeed(-playerSlowAmount, 1f); // cria um metodo para reduzir velocidade por 1 segundo

                
                // ====== KNOCKBACK NO PLAYER ======
                Vector2 knockDir = (collision.transform.position - transform.position).normalized;

                player.ApplyKnockback(knockDir, knockbackForce);
            }


            // Empurrao na propria rocha (pra ela quicar)
            Vector2 rockKnockDir = (transform.position - collision.transform.position).normalized;
            rb.AddForce(rockKnockDir * knockbackForce, ForceMode2D.Impulse);
        }

        // Se colidir com limites ou chao, pode inverter a direcao
        else
        {
            rb.linearVelocity = -rb.linearVelocity;
        }
    }

    void Awake()
    {
        // sorteia posicao vertical antes do Start
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + Random.Range(-3f, 3f),
            transform.position.z
        );
    }

}

