using UnityEngine;

public class NaveSoundController : MonoBehaviour
{
    public AudioSource audioSource;

    // Chamado pela Animation Event
    public void PlayNaveSound()
    {
        audioSource.Play();
    }
}

