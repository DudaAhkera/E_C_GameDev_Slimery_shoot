using UnityEngine;
using System.Collections.Generic;

public class Advanced_Spawner : MonoBehaviour
{
    [Header("Configuracoes Basicas")]
    public List<GameObject> spawnPrefabs;   // Lista de prefabs possíveis
    public int maxObjects = 10;             // Limite de objetos ativos
    public string objectTag = "Rock";  // TAG que esse spawner vai monitorar


    [Header("Intervalo de Spawn (segundos)")]
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 3f;

    [Header("Área de Spawn Aleatória")]
    public Vector2 areaSize = new Vector2(5f, 2f); // Largura e altura da área de spawn
    public bool useRandomPosition = true;

    [Header("Movimento com o Player")]
    public Transform player;     // Referência do player
    private float offsetX;       // Distância inicial no eixo X entre o spawner e o player
    public bool followY = false; // Se quiser que siga também no eixo Y

    private float nextSpawnTime;

    void Start()
    {
        ScheduleNextSpawn();

        // Guarda a distância inicial do player
        if (player != null)
            offsetX = transform.position.x - player.position.x;
    }

    void Update()
    {
        FollowPlayer();

        if (Time.time >= nextSpawnTime && CountObjects() < maxObjects)
        {
            SpawnRandomObject();
            ScheduleNextSpawn();
        }
    }

    void FollowPlayer()
    {
        // Faz o spawner seguir o player
        if (player == null) return;

        float newX = player.position.x + offsetX;
        float newY = followY ? player.position.y : transform.position.y;

        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    void SpawnRandomObject()
    {
        if (spawnPrefabs.Count == 0) return;

        // Escolhe um prefab aleatório
        GameObject prefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Count)];

        // Define posição de spawn
        Vector3 spawnPos = transform.position;
        if (useRandomPosition)
        {
            spawnPos += new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(-areaSize.y / 2, areaSize.y / 2),
                0f
            );
        }

        Instantiate(prefab, spawnPos, Quaternion.identity);

        Debug.Log("Rock spawnou em: " + spawnPos);

    }

    void ScheduleNextSpawn()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

    int CountObjects()
    {
        // Conta quantos objetos com a tag Enemy estão na cena
        return GameObject.FindGameObjectsWithTag(objectTag).Length;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, areaSize.y, 0));
    }
}

