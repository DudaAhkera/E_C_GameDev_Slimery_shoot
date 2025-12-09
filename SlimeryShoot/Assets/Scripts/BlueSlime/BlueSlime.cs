using UnityEngine;

public class BlueSlime : MonoBehaviour
{
    [Header("Configuracoes do Slime")]
    public float speed = 2f;              // velocidade para andar para a esquerda

    [Header("Rastro")]
    public GameObject trailPrefab;        // seu Square azul
    public float trailSpawnInterval = 0.2f; // intervalo entre rastros
    public float trailLifetime = 1.5f;

    private float trailTimer = 0f;

    void Update()
    {
        // mover pra esquerda
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // criar rastro com intervalo
        trailTimer += Time.deltaTime;
        if (trailTimer >= trailSpawnInterval)
        {
            if (trailPrefab != null)
            {
                var trail = Instantiate(trailPrefab, transform.position, Quaternion.identity);
                Destroy(trail, trailLifetime);
            }
            trailTimer = 0f;
        }
    }

}

