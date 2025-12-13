using UnityEngine;


public class ProgressTracker : MonoBehaviour
{

    public static int SlimeKills { get; private set; }
    public static int BlueSlimes { get; private set; }
    public static int BossKills { get; private set; }

    public delegate void ProgressUpdated();
    public static event ProgressUpdated OnProgressUpdated;

    public static void ResetProgress()
    {
        SlimeKills = 0;
        BlueSlimes = 0;
        BossKills = 0;
        OnProgressUpdated?.Invoke();
    }

    public static void RegisterSlimeKill()
    {
        SlimeKills++;
        CheckObjectives();
        OnProgressUpdated?.Invoke();
    }

    public static void RegisterBlueSlimeCollect()
    {
        BlueSlimes++;
        CheckObjectives();
        OnProgressUpdated?.Invoke();
    }

    public static void RegisterBossKill()
    {
        BossKills++;
        CheckObjectives();
        OnProgressUpdated?.Invoke();
    }
    
    private static void CheckObjectives()
    {

        if (DifficultyManager.Instance == null)
        {
            Debug.LogWarning("DifficultyManager não encontrado!");
            return;
        }

        var goals = DifficultyManager.Instance.Objectives;

        if (goals.slimeKillsRequired == 0 &&
            goals.blueSlimesRequired == 0 &&
            goals.bossKillsRequired == 0)
            return;

        bool slimesOk = SlimeKills >= goals.slimeKillsRequired;
        bool bluesOk = BlueSlimes >= goals.blueSlimesRequired;
        bool bossesOk = BossKills >= goals.bossKillsRequired;

        if (MenuController.isTransitioning)
            return;

        if (slimesOk && bluesOk && bossesOk)
        {
            MenuController.GoToNextLevelStatic();
        }
    }

    public static void TriggerGameOver()
    {
        MenuController.GoToGameOverStatic();
    }
}

