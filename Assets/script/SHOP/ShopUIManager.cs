using TMPro;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject shopUI;
    public TextMeshProUGUI feedbackText;

    [Header("Player Gold (테스트용)")]
    public int playerGold = 5000;

    void Update()
    {
        // ESC 키로 상점 닫기
        if (shopUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            shopUI.SetActive(false);
        }
    }

    /// <summary>
    /// 아이템 구매 시도
    /// </summary>
    public void TryBuyItem(int price)
    {
        if (playerGold >= price)
        {
            playerGold -= price;
            Debug.Log($"✅ 아이템 구매 성공! 남은 골드: {playerGold}");

            // TODO: 인벤토리 추가 연결
        }
        else
        {
            ShowFeedback("❌ 잔액이 부족합니다!");
        }
    }

    /// <summary>
    /// 피드백 메시지 표시
    /// </summary>
    private void ShowFeedback(string message)
    {
        if (feedbackText == null) return;

        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideFeedback));
        Invoke(nameof(HideFeedback), 1.5f);
    }

    /// <summary>
    /// 피드백 숨기기
    /// </summary>
    private void HideFeedback()
    {
        feedbackText.gameObject.SetActive(false);
    }
}
