using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameHUDManager : MonoBehaviour
{
    public static GameHUDManager Instance;

    [Header("Referencias UI")]
    public Slider playerHealthBar;
    public Slider bossHealthBar;
    public TextMeshProUGUI pointsText;

    private int totalPoints = 0;

    private bool blinking = false;

    public CanvasGroup healthBarCanvas; // arraste seu painel da barra de vida aqui

    private void Awake()
    {
        Instance = this;

        if (bossHealthBar != null)
            bossHealthBar.gameObject.SetActive(false);

        ProgressTracker.OnProgressUpdated += UpdateObjectivesHUD;
        UpdateObjectivesHUD();
    }

    // ===================== PLAYER =====================
    public void SetPlayerHealth(float current, float max)
    {
        if (playerHealthBar == null) return;
        playerHealthBar.value = current / max;
    }

    // ===================== BOSS =====================
    public void SetBossHealth(float current, float max)
    {
        if (bossHealthBar == null) return;

        // ativa UI somente no primeiro hit
        if (!bossHealthBar.gameObject.activeSelf)
            bossHealthBar.gameObject.SetActive(true);

        bossHealthBar.value = current / max;

        // desliga slider quando morrer
        if (current <= 0)
            bossHealthBar.gameObject.SetActive(false);

    }

    // ===================== PONTUAÇÃO =====================
    public void AddPoints(int amount)
    {
        totalPoints += amount;

        if (pointsText != null)
            pointsText.text = $"Pontos: {totalPoints}";
    }

    public int CurrentScore => totalPoints;

    private void OnDestroy()
    {
        ProgressTracker.OnProgressUpdated -= UpdateObjectivesHUD;
    }

    public void StartHealthBlink()
    {
        if (!blinking)
            StartCoroutine(HealthBlinkCoroutine());
    }

    public void StopHealthBlink()
    {
        blinking = false;
        if (healthBarCanvas != null)
            healthBarCanvas.alpha = 1f;
    }

    private IEnumerator HealthBlinkCoroutine()
    {
        blinking = true;

        while (blinking)
        {
            if (healthBarCanvas != null)
                healthBarCanvas.alpha = Mathf.Abs(Mathf.Sin(Time.time * 6f));

            yield return null;
        }

        if (healthBarCanvas != null)
            healthBarCanvas.alpha = 1f;
    }

    // ===================== BLUE SLIMES COLETADOS =====================
    [Header("Coletaveis")]
    public TextMeshProUGUI blueSlimeText;   // o numero no HUD
    public Image blueSlimeIcon;             // opcional, caso tenha um icone no HUD

    private int totalBlueSlimes = 0;

    public void AddBlueSlime(int amount = 1)
    {
        totalBlueSlimes += amount;

        if (blueSlimeText != null)
            blueSlimeText.text = totalBlueSlimes.ToString();
    }

    public int CurrentBlueSlimes => totalBlueSlimes;

    // ===================== OBJETIVOS =====================
    [Header("Objetivos")]
    public TextMeshProUGUI slimeObjectiveText;
    public TextMeshProUGUI blueSlimeObjectiveText;
    public TextMeshProUGUI bossObjectiveText;

    private void UpdateObjectivesHUD()
    {
        var goals = DifficultyManager.Instance.Objectives;

        if (slimeObjectiveText != null)
        {
            slimeObjectiveText.text = $"Slimes: {ProgressTracker.SlimeKills}/{goals.slimeKillsRequired}";
            slimeObjectiveText.color = ProgressTracker.SlimeKills >= goals.slimeKillsRequired ? Color.green : Color.red;
            slimeObjectiveText.gameObject.SetActive(goals.slimeKillsRequired > 0);
        }

        if (blueSlimeObjectiveText != null)
        {
            blueSlimeObjectiveText.text = $"Blue Slimes: {ProgressTracker.BlueSlimes}/{goals.blueSlimesRequired}";
            blueSlimeObjectiveText.color = ProgressTracker.BlueSlimes >= goals.blueSlimesRequired ? Color.green : Color.red;
            blueSlimeObjectiveText.gameObject.SetActive(goals.blueSlimesRequired > 0);
        }

        if (bossObjectiveText != null)
        {
            bossObjectiveText.text = $"Bosses: {ProgressTracker.BossKills}/{goals.bossKillsRequired}";
            bossObjectiveText.color = ProgressTracker.BossKills >= goals.bossKillsRequired ? Color.green : Color.red;
            bossObjectiveText.gameObject.SetActive(goals.bossKillsRequired > 0);
        }
    }

}



