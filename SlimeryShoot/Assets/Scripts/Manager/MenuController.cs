using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public string[] gameSceneName;
    public static int currentLevelIndex = 0;
    public static string lastPlayedScene; 
    public static MenuController Instance { get; private set; }

    public void OnPlay()
    {
        currentLevelIndex = 0; // começa sempre da primeira fase
        lastPlayedScene = gameSceneName[currentLevelIndex];   // salva a fase inicial
        SceneManager.LoadScene(lastPlayedScene);
    }

    // Botão com dificuldade (recebe 0 Easy, 1 Medium, 2 Hard)
    public void StartGameWithDifficulty(int level)
    {
        string scene = "Main"; // padrão

        if (level == 0) scene = "Main";   // Fácil
        if (level == 1) scene = "Main1";  // Médio
        if (level == 2) scene = "Main2";  // Difícil

        // salva a cena escolhida
        lastPlayedScene = scene;
        Debug.Log("Última cena salva: " + lastPlayedScene);

        SceneManager.LoadScene(scene);
    }

    public static void AdvanceLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < Instance.gameSceneName.Length)
        {
            // próxima fase
            string nextScene = Instance.gameSceneName[currentLevelIndex];

            // só salva se não for GameOver
            if (nextScene != "GameOver")
                lastPlayedScene = nextScene;

            SceneManager.LoadScene(nextScene);
        }
        else
        {
            // acabaram as fases, vai para Vitória
            SceneManager.LoadScene("Vitória");
        } 
    }

    public static void GoToGameOver()
    {
        // não atualiza lastPlayedScene aqui, para manter a última fase jogada
        SceneManager.LoadScene("GameOver");
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}


