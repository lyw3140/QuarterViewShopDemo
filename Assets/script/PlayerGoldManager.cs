using TMPro;
using UnityEngine;

public class PlayerGoldManager : MonoBehaviour
{
    public int currentGold = 0;

    public int CurrentGold => currentGold; // ✅ 읽기 전용 프로퍼티 추가

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateUI();
    }

    public void SpendGold(int amount)
    {
        currentGold -= amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        // 골드 텍스트 갱신
        goldText.text = currentGold.ToString();
    }

    public TextMeshProUGUI goldText; // 기존에 연결해둔 Text UI
}
