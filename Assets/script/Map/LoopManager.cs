using TMPro;
using UnityEngine;

public class LoopManager : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI loopText;
    public PortalUIManager portalUIManager; // 🔹 추가: 포탈 UI 매니저 연결

    [Header("루프 수치")]
    public int currentLoop = 1;
    public int maxLoop = 10;

    private void Start()
    {
        UpdateLoopUI();
    }

    public void GoToNextLoop()
    {
        if (currentLoop < maxLoop)
        {
            currentLoop++;
            UpdateLoopUI();
            Debug.Log($"🌀 루프 증가: {currentLoop} / {maxLoop}");

            // 🔹 루프 증가 후 포탈 UI 표시
            if (portalUIManager != null)
            {
                portalUIManager.ShowPortalUI();
            }
            else
            {
                Debug.LogWarning("⚠️ PortalUIManager가 연결되지 않았습니다.");
            }
        }
        else
        {
            Debug.Log("❌ 최대 루프 도달");
        }
    }

    private void UpdateLoopUI()
    {
        if (loopText != null)
        {
            loopText.text = $"🌀 {currentLoop} / {maxLoop}";
        }
    }
}
