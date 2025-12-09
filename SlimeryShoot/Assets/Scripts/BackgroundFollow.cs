using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [Header("Referências")]
    public Transform cameraTransform;    // Câmera que segue o player
    [Range(0f, 1f)]
    public float parallaxFactor = 0.2f; // Quanto o fundo se move junto da câmera

    private Vector3 startPos;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        startPos = transform.position;
    }

    void LateUpdate()
    {
        // Calcula deslocamento proporcional à câmera
        Vector3 delta = cameraTransform.position - startPos;

        // Move o fundo apenas parcialmente (parallax)
        transform.position = startPos + new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);
    }
}


