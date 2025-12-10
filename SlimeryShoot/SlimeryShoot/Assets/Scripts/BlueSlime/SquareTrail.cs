using UnityEngine;

public class SquareTrail : MonoBehaviour
{
    public float fadeTime = 1.2f;   // tempo para desaparecer

    private SpriteRenderer sr;
    private float timer = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        float alpha = 1f - (timer / fadeTime);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (timer >= fadeTime)
        {
            Destroy(gameObject);
        }
    }
}

