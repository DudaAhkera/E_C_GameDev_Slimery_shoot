using UnityEngine;
using UnityEngine.UI;

public class AmmoIconController : MonoBehaviour
{
    public Image ammoIcon;          // Ícone no HUD
    public Color normalColor = Color.white; 
    public Color emptyColor = Color.gray;    // Cor quando sem munição
    public float blinkSpeed = 2f;   // Velocidade do piscar

    private bool isEmpty = false;

    void Update()
    {
        if (isEmpty)
        {
            // Faz o ícone piscar entre transparente e cinza
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            ammoIcon.color = new Color(emptyColor.r, emptyColor.g, emptyColor.b, alpha);
        }
    }

    public void SetEmpty(bool empty)
    {
        isEmpty = empty;

        if (!empty)
        {
            ammoIcon.color = normalColor; // volta ao normal
        }
    }
}

