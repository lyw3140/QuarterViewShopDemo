using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoopHUDManager : MonoBehaviour
{
    public Image healthFillImage;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI loopText;
    public TextMeshProUGUI rewardText;

    public int currentLoop = 1;
    public int maxLoop = 10;
    public float loopDuration = 60f;
    public string baseSceneName = "BaseScene";

    private float timer = 0f;
    private bool isLoopActive = true;

    void Start()
    {
        PlayerGoldManager.Instance?.LoadGold(); // 골드 불러오기
        currentLoop = PlayerPrefs.GetInt("LoopCount", 1);

        SetGold(PlayerGoldManager.Instance.CurrentGold);
        UpdateLoopText();
        ClearRewardText();
    }

    void Update()
    {
        if (!isLoopActive) return;

        timer += Time.deltaTime;
        float remainingTime = Mathf.Max(loopDuration - timer, 0);
        timerText.text = FormatTime(remainingTime);

        if (timer >= loopDuration)
        {
            EndLoop();
        }
    }

    public void SetHealth(float currentHP, float maxHP)
    {
        float ratio = Mathf.Clamp01(currentHP / maxHP);
        if (healthFillImage != null)
            healthFillImage.fillAmount = ratio;
    }

    public void SetGold(int gold)
    {
        if (goldText != null)
            goldText.text = $"골드: {gold}";
    }

    public void ShowReward(string rewardSummary)
    {
        if (rewardText != null)
            rewardText.text = rewardSummary;
    }

    public void ClearRewardText()
    {
        if (rewardText != null)
            rewardText.text = "";
    }

    void UpdateLoopText()
    {
        if (loopText != null)
            loopText.text = $"루프: {currentLoop} / {maxLoop}";
    }

    void EndLoop()
    {
        isLoopActive = false;
        currentLoop++;

        SaveData();
        SceneManager.LoadScene(baseSceneName);
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("LoopCount", currentLoop);
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        PlayerGoldManager.Instance?.SaveGold(); // 골드 저장
        PlayerPrefs.Save();
        Debug.Log("✅ PlayerPrefs 저장 완료");
    }

    string FormatTime(float time)
    {
        int m = Mathf.FloorToInt(time / 60f);
        int s = Mathf.FloorToInt(time % 60f);
        return $"{m:00}:{s:00}";
    }
}
