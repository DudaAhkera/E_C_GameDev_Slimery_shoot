using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public LevelObjectives Objectives { get; private set; }

    private void Awake()
    {
        Instance = this;
        SetObjectivesForScene(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetObjectivesForScene(scene.name);
    }

    private void SetObjectivesForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Main":   // Fase 1
                Objectives = new LevelObjectives(25, 0, 0);
                break;

            case "Main1":  // Fase 2
                Objectives = new LevelObjectives(40, 15, 0);
                break;

            case "Main2":  // Fase 3
                Objectives = new LevelObjectives(60, 30, 5);
                break;

            default:
                Objectives = new LevelObjectives(0, 0, 0);
                break;
        }
    }

}

[System.Serializable]
public class LevelObjectives
{
    public int slimeKillsRequired;
    public int blueSlimesRequired;
    public int bossKillsRequired;

    public LevelObjectives(int slimes, int blues, int bosses)
    {
        slimeKillsRequired = slimes;
        blueSlimesRequired = blues;
        bossKillsRequired = bosses;
    }
}


