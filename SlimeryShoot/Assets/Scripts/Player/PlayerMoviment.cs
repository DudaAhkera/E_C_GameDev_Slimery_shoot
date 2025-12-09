using UnityEngine;
using System.Collections;


public class PlayerMoviment : MonoBehaviour
{
    // ====== CONFIGURACOES DE MOVIMENTO ======
    public float speed = 5f; //velocidade da nave


    // ====== CONFIGURACOES DE INCLINACAO ======
    [Header("Inclinacao da Nave")]
    public float tiltAmount = 15f; //Quanto a nave inclina em graus)
    public float tiltSpeed = 5f; //Velocidade com que a nave inlcina/retorna

    // ====== COMPONENTES ======
    private Rigidbody2D rb; //Referencia ao rigidbody 2d na nave
    private Vector2 move; //Direcao atual do movimento

    // ====== KNOCKBACK ======
    private bool isKnockback = false;

    private PlayerSoundController sounds;

    
    void Start()
    {
        //Pega o componente Rigidbody2D no inicio do jogo
        rb = GetComponent<Rigidbody2D>();

        sounds = FindObjectOfType<PlayerSoundController>();
        
    }

 
    void Update()
    {
        // ====== CAPTURA DE ENTRADA ======

        if (isKnockback) return; // enquanto estiver levando pancada, ignora inputs

        //Recebe os Inputs do teclado (ou controle) para o eixo horizontal e vertical
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //Normaliza o vetor para evitar que o movimento na diagonal seja mais rapido
        move = new Vector2(h, v).normalized;

        // ====== SONS DE MOVIMENTO ======
        bool movingForward = (v > 0) || (h > 0);
        bool movingBackward = (v < 0) || (h < 0);

        if (movingForward)
        {
            sounds.PlayMoveFront();
        }
        else if (movingBackward)
        {
            sounds.PlayMoveBack();
        }
        else
        {
            sounds.StopMovementSounds();
        }

        // ====== INCLINACAO DA NAVE ======
        //Define a rotacao desejada com base na direcao horizontal
        float targetRotationZ = -h * tiltAmount;

        // Aplica uma transicao suave da rotacao atual para a desejada
        float newZ = Mathf.LerpAngle(transform.rotation.eulerAngles.z, targetRotationZ, Time.deltaTime * tiltSpeed);

        //Atualiza a rotacao da nave
        transform.rotation = Quaternion.Euler(0, 0, newZ);


    }

    void FixedUpdate()
    {
        if (isKnockback) return; // não move durante o knockback

        //Define a velocidade   da nave com base no vetor de movimento e na velocidade configurada
        rb.linearVelocity = move * speed;
    }


     // ====== REDUZ VELOCIDADE NO IMPACTO DA ROCHA ======
     public void AdjustSpeed(float amount, float duration = 2f)
    {
        StartCoroutine(AdjustSpeedCoroutine(amount, duration));
    }

    private IEnumerator AdjustSpeedCoroutine(float amount, float duration)
    {
        speed += amount;  // reduz ou aumenta
        yield return new WaitForSeconds(duration);
        speed -= amount;  // volta ao normal
    }

    // ====== KNOCKBACK ======
    public void ApplyKnockback(Vector2 direction, float force, float duration = 0.15f)
    {
        StartCoroutine(ApplyKnockbackCoroutine(direction, force, duration));
    }

    private IEnumerator ApplyKnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        isKnockback = true;

        rb.linearVelocity = Vector2.zero; // zera antes de aplicar impulso
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        isKnockback = false;
    }
}
