using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SlimeImgSound : MonoBehaviour
{
    private AudioSource audioSource;
    private SlimeShake shake;
    private bool soundPlayed = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        shake = GetComponent<SlimeShake>(); 
    }

    void Update()
    {
        if (soundPlayed) return;

        if (shake == null)
        {
            // opcional: tenta achar o SlimeShake no pai, caso nao esteja no mesmo objeto
            shake = GetComponentInParent<SlimeShake>();
            if (shake == null) return;
        }

        // usa o getter publico TargetScale
        if (shake.TargetScale > 0f)
        {
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
            soundPlayed = true;
        }
    }
}

