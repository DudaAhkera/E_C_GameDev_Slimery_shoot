using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class SlimeTrash_Monster : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 3f;
    public float minDistance = 0.6f;
    public float swarmRadius = 0.8f;           // raio do aglomerado ao redor do player
    public float approachSmoothing = 8f;       // suavização do steering
    public float separationForce = 0.5f;       // força para evitar sobreposição extrema

    [Header("Tilt visual (lento)")]
    public bool enableTilt = true;
    public float tiltAmount = 6f;
    public float tiltSpeed = 1.2f;

    [Header("Ataque")]
    public float damagePerSecond = 10f;        // dano contínuo no player
    public int pointsValue = 5;                // pontos ao morrer

    private Transform player;
    private Rigidbody2D rb;
    private float tiltTimer = 0f;
    private bool isHit = false;

    // Fase única por slime para variar a posição no enxame
    private float phaseOffset;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.freezeRotation = false;
            rb.isKinematic = false;
        }

        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;

        phaseOffset = Random.Range(0f, 2f * Mathf.PI);
        moveSpeed += Random.Range(-0.25f, 0.25f);

    }

    void Update()
    {
        if (player == null || rb == null || isHit) return;

        Vector2 toPlayer = (Vector2)player.position - rb.position;
        float distance = toPlayer.magnitude;

        // Alvo dinâmico em volta do player (swarm)
        float phase = (Time.time * 0.8f) + phaseOffset;
        Vector2 swarmOffset = new Vector2(Mathf.Cos(phase), Mathf.Sin(phase)) * swarmRadius;
        Vector2 targetPos = (Vector2)player.position + swarmOffset;

        Vector2 desired = (targetPos - rb.position);
        Vector2 desiredDir = desired.sqrMagnitude > 0.0001f ? desired.normalized : Vector2.zero;

        float speedScale = distance > minDistance ? 1f : 0.6f;

        // Separação leve de vizinhos
        Vector2 separation = ComputeSeparation(0.7f) * separationForce;

        // Steering final com suavização
        Vector2 targetVelocity = (desiredDir * moveSpeed * speedScale) + separation;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, Time.deltaTime * approachSmoothing);

        // Flip visual
        transform.localScale = (toPlayer.x < 0f) ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);

        // Tilt lento e suave (simula “andar”)
        if (enableTilt)
        {
            tiltTimer += Time.deltaTime * tiltSpeed;
            float zRotation = Mathf.Sin(tiltTimer + phaseOffset) * tiltAmount;
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
        }
    }

    private Vector2 ComputeSeparation(float neighborDist)
    {
        // Evita sobreposição ao somar uma leve força de separação dos slimes próximos
        Vector2 steer = Vector2.zero;
        int count = 0;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
        {
            if (e == gameObject) continue;
            Vector2 diff = rb.position - (Vector2)e.transform.position;
            float d = diff.magnitude;
            if (d > 0f && d < neighborDist)
            {
                steer += diff.normalized / Mathf.Max(d, 0.05f); // mais perto => mais força
                count++;
            }
        }

        if (count > 0) steer /= count;
        return steer;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isHit && collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }

    public void TakeHit()
    {
        if (rb == null || isHit) return;

        isHit = true;
        rb.linearVelocity = Vector2.zero;

        // Knockback contido para não arremessar para fora da cena
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.3f, 1f)).normalized;
        rb.AddForce(randomDirection * 3f, ForceMode2D.Impulse);
        rb.angularVelocity = Random.Range(140f, 260f);

        Destroy(gameObject, 1.2f);

        if (GameHUDManager.Instance != null)
            GameHUDManager.Instance.AddPoints(pointsValue);

        // Ao acertar slime
        ProgressTracker.RegisterSlimeKill();
    }

}







