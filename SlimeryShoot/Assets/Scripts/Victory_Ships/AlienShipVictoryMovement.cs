using UnityEngine;

public class AlienShipVictoryMovement : MonoBehaviour
{
    [Header("Movimento ate o alvo")]
    public Transform targetPosition;
    public float moveSpeed = 3f;

    [Header("Inclinacao")]
    public float tiltAmount = 20f;
    public float tiltSpeed = 5f;

    [Header("Flutuacao")]
    public float floatAmplitude = 0.2f;
    public float floatFrequency = 2f;
    public float floatSmoothTime = 1f;   // tempo para aumentar a amplitude (TRANSICAO SUAVE)

    private bool reachedTarget = false;
    private bool startedFloating = false;

    private float currentFloatAmplitude = 0f;  // amplitude começa 0 SEM SALTO
    private Vector3 floatBasePos;

    void Update()
    {
        if (!reachedTarget)
            MoveToTarget();
        else
            FloatAround();
    }

    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition.position) > 0.05f)
        {
            Vector3 direction = (targetPosition.position - transform.position).normalized;

            float tilt = direction.x * -tiltAmount;

            Quaternion targetRot = Quaternion.Euler(0, 0, tilt);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, tiltSpeed * Time.deltaTime);
        }
        else
        {
            reachedTarget = true;
        }
    }

    void FloatAround()
    {
        // primeira vez que entra aqui
        if (!startedFloating)
        {
            // mantem posicao final sem pular
            floatBasePos = transform.position;

            // suaviza a rotacao ate ficar reta
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 3f);

            // aumenta a amplitude aos poucos
            currentFloatAmplitude = Mathf.Lerp(currentFloatAmplitude, floatAmplitude, Time.deltaTime * floatSmoothTime);

            // ativa floating quando estiver suave
            if (Mathf.Abs(currentFloatAmplitude - floatAmplitude) < 0.01f)
            {
                currentFloatAmplitude = floatAmplitude;
                startedFloating = true;
            }

            return; // ainda nao flutua, esta transicionando
        }

        // movimento senoidal suave
        float y = Mathf.Sin(Time.time * floatFrequency) * currentFloatAmplitude;
        float x = Mathf.Cos(Time.time * floatFrequency * 0.5f) * currentFloatAmplitude * 0.3f;

        transform.position = floatBasePos + new Vector3(x, y, 0);
    }
}


