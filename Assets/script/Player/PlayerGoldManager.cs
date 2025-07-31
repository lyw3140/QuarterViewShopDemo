
using TMPro;
using UnityEngine;

public class PlayerGoldManager : MonoBehaviour
{
    public static PlayerGoldManager Instance;

    [Header("UI 연결")]
    public TextMeshProUGUI goldText;

    [Header("현재 골드")]
    public int currentGold = 0;

    public int CurrentGold => currentGold;

    private void Awake()
    {
        // 싱글톤
        if (Instance == null)
        {
            Instance = this;

            // 🔧 씬 전환 시 유지 안 하도록 수정
            // DontDestroyOnLoad(gameObject); // 🔴 이 줄을 주석 처리함

            LoadGold(); // 시작 시 골드 불러오기
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateUI();
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateUI();
            return true;
        }
        else
        {
            Debug.Log("❌ 골드 부족!");
            return false;
        }
    }

    void UpdateUI()
    {
        if (goldText != null)
            goldText.text = $"Gold: {currentGold}";
    }

    public void SaveGold()
    {
        PlayerPrefs.SetInt("CurrentGold", currentGold);
        PlayerPrefs.Save();
        Debug.Log("💾 골드 저장 완료");
    }

    public void LoadGold()
    {
        currentGold = PlayerPrefs.GetInt("CurrentGold", 0);
        UpdateUI();
        Debug.Log($"📥 골드 불러오기 완료: {currentGold}G");
    }
}
