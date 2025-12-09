using UnityEngine;


public class ProgressTracker : MonoBehaviour
{
    public static ProgressTracker Instance;

    public int SlimeKills { get; private set; }
    public int BlueSlimes { get; private set; }
    public int BossKills { get; private set; }

    public delegate void ProgressUpdated();
    public event ProgressUpdated OnProgressUpdated;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ResetProgress();
    }

    public void ResetProgress()
    {
        SlimeKills = 0;
        BlueSlimes = 0;
        BossKills = 0;
    }

    public void RegisterSlimeKill()
    {
        SlimeKills++;
        CheckObjectives();
        OnProgressUpdated?.Invoke();
    }

    public void RegisterBlueSlimeCollect()
    {
        BlueSlimes++;
        CheckObjectives();
        OnProgressUpdated?.Invoke();
    }

    public void RegisterBossKill()
    {
        BossKills++;
        CheckObjectives();
        OnProgressUpdated?.Invoke();
    }
    
    private void CheckObjectives()
    {
        var goals = DifficultyManager.Instance.Objectives;

        bool slimesOk = SlimeKills >= goals.slimeKillsRequired;
        bool bluesOk = BlueSlimes >= goals.blueSlimesRequired;
        bool bossesOk = BossKills >= goals.bossKillsRequired;

        if (slimesOk && bluesOk && bossesOk)
        {
            // Avança para a próxima fase
            MenuController.AdvanceLevel();
        }
    }
}

