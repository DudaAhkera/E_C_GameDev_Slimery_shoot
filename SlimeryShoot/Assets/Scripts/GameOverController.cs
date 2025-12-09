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

        public void OnReplay()
    {
        if (!string.IsNullOrEmpty(MenuController.lastPlayedScene))
            SceneManager.LoadScene(MenuController.lastPlayedScene);
        else
            SceneManager.LoadScene("Main"); // fallback de segurança
    }


    public void OnMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}

