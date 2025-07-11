using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoopHUDManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI timerText;
    public Slider healthBar;

    private PlayerGoldManager goldManager;
    private PlayerHealth playerHealth;

    void Start()
    {
        goldManager = FindObjectOfType<PlayerGoldManager>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    void Update()
    {
        // 타이머 표시
        if (timerText != null)
            timerText.text = $"⏱ {Time.time:F1}초";

        // 골드 표시 (LoopHUD에 별도로 보여주는 텍스트)
        if (goldManager != null && goldText != null)
            goldText.text = $"💰 {goldManager.CurrentGold}";

        // 체력 표시 (Slider로 처리)
        if (playerHealth != null && healthBar != null)
            healthBar.value = (float)playerHealth.currentHealth / playerHealth.maxHealth;
    }
}
