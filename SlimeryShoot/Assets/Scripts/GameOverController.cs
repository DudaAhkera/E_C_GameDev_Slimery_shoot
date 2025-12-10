using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestText;

    [Header("Cenas")]
    public string mainSceneName = "Main";
    public string menuSceneName = "Menu";

    void Start()
    {
        int last = PlayerPrefs.GetInt("LastScore", 0);
        int best = PlayerPrefs.GetInt("BestScore", 0);

        if (scoreText != null) scoreText.text = $"Pontuação: {last}";
        if (bestText != null) bestText.text = $"Melhor: {best}";
    }

    // Botão Replay volta para a última fase jogada
    public void OnReplay()
    {
        if (!string.IsNullOrEmpty(MenuController.lastPlayedScene))
        {
            // volta para a última fase jogada (não salva GameOver como última cena)
            SceneManager.LoadScene(MenuController.lastPlayedScene);
        }
        else
        {
            // fallback: se não houver nada salvo, volta para a primeira fase
            SceneManager.LoadScene(mainSceneName);
        }
    }

    // Botão Menu volta para o Menu principal
    public void OnMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}


