using UnityEngine;

public class BG_Float : MonoBehaviour
{
    [Header("Flutuacao")]
    public float floatSpeedX = 0.2f;   // velocidade horizontal da flutuacao
    public float floatSpeedY = 0.1f;   // velocidade vertical da flutuacao
    public float floatAmountX = 0.5f;  // amplitude horizontal
    public float floatAmountY = 0.2f;  // amplitude vertical

    [Header("Zoom suave")]
    public float zoomSpeed = 0.2f;     // velocidade do zoom
    public float zoomAmount = 0.02f;   // intensidade do zoom

    [Header("Rotacao suave")]
    public float rotateSpeed = 0.1f;   // velocidade da rotacao
    public float rotateAmount = 1f;    // intensidade da rotacao em graus

    private Vector3 startPos;
    private Vector3 startScale;
    private Quaternion startRot;

    void Start()
    {
        startPos = transform.position;
        startScale = transform.localScale;
        startRot = transform.rotation;
    }

    void Update()
    {
        // Movimento flutuante
        float newX = startPos.x + Mathf.Sin(Time.time * floatSpeedX) * floatAmountX;
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeedY) * floatAmountY;
        transform.position = new Vector3(newX, newY, startPos.z);

        // Zoom suave
        float scaleOffset = Mathf.Sin(Time.time * zoomSpeed) * zoomAmount;
        transform.localScale = startScale * (1 + scaleOffset);

        // Rotacao suave
        float rotZ = Mathf.Sin(Time.time * rotateSpeed) * rotateAmount;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}

