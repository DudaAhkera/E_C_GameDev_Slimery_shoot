using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BlueSlimeCollectibles : MonoBehaviour
{
    [Header("Collect")]
    public AudioClip collectClip;          // som curto ao coletar (opcional)
    public GameObject collectVFXPrefab;    // efeito visual (opcional)
    public int value = 1;                  // quantos pontos/units incrementa

    private bool collected = false;
    private Collider2D _col;

    private void Awake()
    {
        _col = GetComponent<Collider2D>();
        if (_col != null)
            _col.isTrigger = true; // garante trigger
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        Debug.Log("Algo encostou: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            collected = true;
            Debug.Log("Player encostou!");
            Collect();
        }

    }

    private void Collect()
    {
        Debug.Log("Chamou Collect()");

        // tocar som (se existir)
        if (collectClip != null)
            AudioSource.PlayClipAtPoint(collectClip, Camera.main.transform.position, 1.2f);

        // spawn VFX (se tiver)
        if (collectVFXPrefab != null)
            Instantiate(collectVFXPrefab, transform.position, Quaternion.identity);

        // informar o manager
        if (GameHUDManager.Instance != null)
            GameHUDManager.Instance.AddBlueSlime(value);

        else
            Debug.LogWarning("GameHUDManager.Instance não encontrado na cena.");


        // destruir o objeto
        Destroy(gameObject);

        ProgressTracker.RegisterBlueSlimeCollect();

    }
}

