using UnityEngine;

public class LaserBossSound : MonoBehaviour
{
    public AudioClip laserSound; // Som do laser

    private AudioSource audioSource;

    void Start()
    {
        if (audioSource != null && laserSound != null)
        {
            audioSource.clip = laserSound;
            audioSource.playOnAwake = false;
            audioSource.Play();
        }
    }
}

