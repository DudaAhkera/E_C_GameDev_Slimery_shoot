using UnityEngine;

public class BlueSlimeSpawner : MonoBehaviour
{
    [Header("Configuracao do Spawn")]
    public GameObject slimePrefab;  
    public float spawnInterval = 3f;     // tempo entre spawns
    public float yOffsetRange = 1.5f;    // movimento aleatorio no eixo Y

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnSlime();
            timer = 0f;
        }
    }

    void SpawnSlime()
    {
        // cria desvio vertical aleatorio
        float randomYOffset = Random.Range(-yOffsetRange, yOffsetRange);

        Vector3 spawnPos = new Vector3(
            transform.position.x,
            transform.position.y + randomYOffset,
            transform.position.z
        );

        Instantiate(slimePrefab, spawnPos, Quaternion.identity);
    }
}

