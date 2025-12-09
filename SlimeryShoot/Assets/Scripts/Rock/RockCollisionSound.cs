using UnityEngine;

public class RockCollisionSound : MonoBehaviour
{
    public AudioClip hitSound;   // Som da batida
    public float volume = 1f;    // Ajuste de volume

    private AudioSource audioSource;

    void Start()
    {
        // Cria um AudioSource automaticamente
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D pra ficar audível
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound, volume);
    }
}

