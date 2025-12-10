using UnityEngine;
using System.Collections;

public class SlimeShooter : MonoBehaviour
{
    [Header("Configuracoes de Tiro")]
    public GameObject slimeProjectilePrefab; // Prefab do slime
    public Transform ShootPoint;             // Ponto de onde o slime � disparado
    public float shootForce = 10f;           // Velocidade do tiro
    public float shootRate = 0.2f;           // Intervalo entre tiros

    [Header("Sistema de Recarga")]
    public int maxShotsBeforeCooldown = 20;   // Quantidade de tiros antes da recarga
    public float cooldownTime = 3f;           // Tempo de recarga em segundos

    private float nextShootTime = 0f;
    private int shotsFired = 0;               // Contador de tiros
    private bool isCoolingDown = false;       // Flag de recarga
    private PlayerSoundController sounds;


    public AmmoIconController ammoIconController; // arraste no Inspector

    void Start()
    {
        sounds = FindObjectOfType<PlayerSoundController>();
    }


    void Update()
    {   

        if (isCoolingDown) return; // Bloqueia disparo durante recarga
        // Dispara enquanto segura a tecla espaco
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextShootTime)
        {
            Shoot();

            nextShootTime = Time.time + shootRate;

            shotsFired++;

            // Se atingir o limite de tiros, inicia recarga
            if (shotsFired >= maxShotsBeforeCooldown)
            {
                StartCoroutine(Cooldown());
            }
        }
    }

    void Shoot()
    {

        // Som do tiro
        if (sounds != null)
            sounds.PlayShoot();

        // Instancia o projetil na posicao e rotacao do shootPoint
        GameObject slimeProjectile = Instantiate(slimeProjectilePrefab, ShootPoint.position, ShootPoint.rotation);

        // Adiciona forca para mover o tiro para a direita
        SlimeProjectiles proj = slimeProjectile.GetComponent<SlimeProjectiles>();
        if (proj != null)
        {
            proj.direction = Vector2.right; // Movimento horizontal (X positivo)
        }


    }

    IEnumerator Cooldown()
    {
        isCoolingDown = true;

        if (ammoIconController != null)
        ammoIconController.SetEmpty(true); // ativa piscar

        // Aqui você pode atualizar o HUD para mostrar "Recarregando..."
        Debug.Log("Recarregando...");

        yield return new WaitForSeconds(cooldownTime);

        shotsFired = 0;        // Reseta contador
        isCoolingDown = false; // Libera disparo novamente

        if (ammoIconController != null)
        ammoIconController.SetEmpty(false); // volta ao normal
    }
}

