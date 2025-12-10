using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource shootSource;
    public AudioSource moveFrontSource;
    public AudioSource moveBackSource;

    // ------------ TIRO ------------
    public void PlayShoot()
    {
        if (!shootSource.isPlaying)
            shootSource.Play();
    }

    // ------------ MOVIMENTO FRENTE / CIMA ------------
    public void PlayMoveFront()
    {
        if (!moveFrontSource.isPlaying)
            moveFrontSource.Play();

        moveBackSource.Stop();
    }

    // ------------ MOVIMENTO TRAS / BAIXO ------------
    public void PlayMoveBack()
    {
        if (!moveBackSource.isPlaying)
            moveBackSource.Play();

        moveFrontSource.Stop();
    }

    // ------------ PARAR MOVIMENTO ------------
    public void StopMovementSounds()
    {
        moveFrontSource.Stop();
        moveBackSource.Stop();
    }
}

