using UnityEngine;
using System.Collections;

public class VictorySequenceController : MonoBehaviour
{
    [Header("Objetos da Cena")]
    public GameObject title;
    public GameObject[] alienShips;
    public GameObject messageBox;
    public GameObject replayButton;

    [Header("Mensagem Final")]
    [TextArea(3,5)]
    public string victoryMessage =
        "Muito bem, Comandante Puppy... Você salvou o universo ao desintegrar toneladas de slime lixo!";

    public float titleDelay = 1f;
    public float shipsDelay = 1.5f;
    public float messageDelay = 1f;
    public float buttonDelay = 1f;

    [Header("Som de Vitoria")]
    public AudioSource victorySound;

    private void Start()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // 1) Exibe o título
        yield return new WaitForSeconds(titleDelay);
        title.SetActive(true);

        // 2) Liga as naves
        yield return new WaitForSeconds(shipsDelay);
        foreach (var ship in alienShips)
            ship.SetActive(true);

        // 3) Mostra mensagem datilografada
        yield return new WaitForSeconds(messageDelay);
        messageBox.SetActive(true);

        var typewriter = messageBox.GetComponent<TypewriterText>();
        typewriter.Play(victoryMessage);

        // toca o som de vitoria junto com a mensagem
        if (victorySound != null)
            victorySound.Play();

        // espera aproximada da duração do efeito
        yield return new WaitForSeconds(victoryMessage.Length * typewriter.charDelay);

        // 4) Botão de replay
        yield return new WaitForSeconds(buttonDelay);
        replayButton.SetActive(true);
    }
}


