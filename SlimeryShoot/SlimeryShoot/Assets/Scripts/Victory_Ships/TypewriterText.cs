using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterText : MonoBehaviour
{
    [Header("Referencias")]
    public TextMeshProUGUI textUI;

    [Header("Configuracao")]
    [TextArea(3, 10)]
    public string fullText;

    public float charDelay = 0.05f;
    public float dramaticPauseTime = 0.8f;

    private Coroutine typingCoroutine;

    void Start()
    {
        // Remove o autostart se você só quer iniciar quando o Controller mandar
        // Pode deixar vazio se preferir
        textUI.text = "";
    }

    // NOVO — método para iniciar digitando um texto enviado pelo controller
    public void Play(string newText)
    {
        fullText = newText;
        Play();
    }

    // NOVO — inicia digitando o texto já existente no fullText
    public void Play()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        textUI.text = "";
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textUI.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            // Verifica se tem "<pause>" sem ultrapassar a string
            if (fullText[i] == '<' && i + 7 <= fullText.Length)
            {
                if (fullText.Substring(i, 7) == "<pause>")
                {
                    yield return new WaitForSeconds(dramaticPauseTime);

                    i += 6; // pula "<pause>"
                    continue;
                }
            }

            textUI.text += fullText[i];
            yield return new WaitForSeconds(charDelay);
        }
    }
}



