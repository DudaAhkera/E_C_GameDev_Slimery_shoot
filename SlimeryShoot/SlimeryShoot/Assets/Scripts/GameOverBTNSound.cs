using UnityEngine;

public class GameOverBTNSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        if (audioSource != null)
            audioSource.Play();
    }
}

