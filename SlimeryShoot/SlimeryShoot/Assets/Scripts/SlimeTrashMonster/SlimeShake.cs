using UnityEngine;

public class SlimeShake : MonoBehaviour
{
    [Header("Slime Growth")]
    public float finalScaleX = 6f;     // largura final da poca
    public float finalScaleY = 0.4f;   // altura final da poca
    public float growthPerHit = 0.06f; // quanto cresce por letra
    public float growSpeed = 6f;       // suavidade do crescimento

    private float targetScale = 0f;    // controla de 0 ate 1
    private Vector3 baseScale;

    // Getter publico (somente leitura) para que outros scripts leiam o progresso
    public float TargetScale => targetScale;

    void Start()
    {
        // define o tamanho final da poca
        baseScale = new Vector3(finalScaleX, finalScaleY, 1f);

        // começa invisivel
        transform.localScale = Vector3.zero;
    }

    public void Grow()
    {
        targetScale = Mathf.Clamp01(targetScale + growthPerHit);
    }

    void Update()
    {
        // cresce suavemente ate o tamanho final
        Vector3 desiredScale = baseScale * targetScale;
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * growSpeed);
    }
}





