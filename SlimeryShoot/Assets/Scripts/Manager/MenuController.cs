using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public string[] gameSceneName; // nomes das cenas
    public static int currentLevelIndex = 0;

    public Button replayButton;
    public Button menuButton;
    public Button playButton; 

    public static bool isTransitioning = false;
    public static string lastPlayedScene;

    void Start()
    {
        isTransitioning = false;

        if (playButton != null) playButton.onClick.AddListener(() => StartGame(0));
        if (replayButton != null) replayButton.onClick.AddListener(ReplayLevel);
        if (menuButton != null) menuButton.onClick.AddListener(GoToMenu);
    }

        // Método que vai para o nível anterior ou reinicia a cena de acordo com a necessidade.
    private void ReplayLevel()
    {
        if (!string.IsNullOrEmpty(lastPlayedScene))
            SceneManager.LoadScene(lastPlayedScene);

    }

        // Botão que envia para o Menu principal
    private void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void StartGame(int level)
    {
        level = Mathf.Clamp(level, 0, gameSceneName.Length - 1);
        currentLevelIndex = level;
        lastPlayedScene = gameSceneName[level];

        ProgressTracker.ResetProgress(); // limpa os objetivos
        SceneManager.LoadScene(lastPlayedScene);
    }

    public void AdvanceLevel()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        // se ainda NÃO for a última fase
        if (currentLevelIndex < gameSceneName.Length - 1)
        {
            currentLevelIndex++;
            lastPlayedScene = gameSceneName[currentLevelIndex];

            ProgressTracker.ResetProgress();
            SceneManager.LoadScene(lastPlayedScene);
        }
        else
        {
            // só a Main2 chega aqui
            SceneManager.LoadScene("Victory");
        }
    }
        // Métodos estáticos para serem chamados de ProgressTracker
    public static void GoToNextLevelStatic()
    {
        var controller = GameObject.FindObjectOfType<MenuController>();
        if (controller != null) controller.AdvanceLevel();
    }

    public static void GoToGameOverStatic()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        SceneManager.LoadScene("GameOver");
    }



}



