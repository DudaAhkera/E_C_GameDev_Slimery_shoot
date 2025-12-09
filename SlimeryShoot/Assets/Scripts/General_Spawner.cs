using UnityEngine;

public class General_Spawner : MonoBehaviour
{
    [Header("Configuracoes do Spawner")]
    public GameObject spawnPrefab;      // Prefab do inimigo que será instanciado
    public float spawnInterval = 2f;    // Tempo entre cada spawn
    public int maxObject = 8;          // Quantidade máxima de inimigos ativos
    public Transform spawnPoint;        // Onde o inimigo vai nascer (pode ser o próprio transform)
    public string objectTag = "BossEnemy";  //Tag usada para contar quantos objetos existem na cena


    [Header("Movimento com o Player")]
    public Transform player;             // Player a ser seguido
    private float offsetX;               // Diferença inicial no eixo X entre spawner e player

    [Header("Spawn Vertical Aleatório")]
    public float minY = -2f;             // Limite inferior para spawn
    public float maxY = 2f;              // Limite superior para spawn
    public bool useRandomY = true;       // Ativa/desativa o spawn vertical aleatório

    private float nextSpawnTime = 0f;


    void Start()
    {
        // Calcula a distância inicial entre o player e o spawner
        if (player != null)
            offsetX = transform.position.x - player.position.x;
    }


    void Update()
    {
        // Faz o spawner seguir o player na horizontal, mantendo a distância inicial
        if (player != null)
        {
            transform.position = new Vector3(player.position.x + offsetX, transform.position.y, transform.position.z);
        }

        // Verifica se já é hora de spawnar e se há espaço para mais inimigos
        if (Time.time >= nextSpawnTime && CountObjects() < maxObject)
        {
            SpawnObject();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnObject()
    {
        if (spawnPrefab == null) return;

        // Define o ponto base do spawn
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;

        // Adiciona variação vertical, se ativado
        if (useRandomY)
        {
            float randomY = Random.Range(minY, maxY);
            spawnPos.y += randomY;
        }

        Instantiate(spawnPrefab, spawnPos, spawnPrefab.transform.rotation);
    }

    int CountObjects()
    {
        // Conta quantos inimigos com a mesma tag existem na cena
        return GameObject.FindGameObjectsWithTag(objectTag).Length;
    }

        // Exibe o range vertical no editor (ajuda visual)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + maxY, transform.position.z),
                        new Vector3(transform.position.x, transform.position.y + minY, transform.position.z));
    }

}