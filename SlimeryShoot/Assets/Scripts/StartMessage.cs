using UnityEngine;
using TMPro;
using System.Collections;

public class StartMessage : MonoBehaviour
{
    [Header("Configurações da Mensagem")]
    public TextMeshProUGUI messageText;   // o TMP do texto
    [TextArea] public string fullMessage = "Acerte os slime trash monsters!";
    public float typeSpeed = 0.05f;       // velocidade por letra
    public float timeOnScreenAfter = 1f;  // tempo após terminar de escrever

    void Start()
    {
        StartCoroutine(ShowMessage());
    }

    IEnumerator ShowMessage()
    {
        messageText.gameObject.SetActive(true);
        messageText.text = "";

        // Digitar letra por letra
        foreach (char c in fullMessage)
        {
            messageText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        // Espera um tempinho após terminar de escrever
        yield return new WaitForSeconds(timeOnScreenAfter);

        // Some a mensagem
        messageText.gameObject.SetActive(false);
    }
}


