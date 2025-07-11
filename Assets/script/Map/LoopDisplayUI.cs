using TMPro;
using UnityEngine;

public class LoopDisplayUI : MonoBehaviour
{
    public TextMeshProUGUI loopText;
    public TextMeshProUGUI rewardText; // ✅ 보상 텍스트 추가

    [Header("Loop Info")]
    public int currentLoop = 1;
    public int maxLoop = 10;

    void Start()
    {
        UpdateLoopText();
        ClearRewardText(); // 시작 시 보상 텍스트 비우기
    }

    public void UpdateLoop(int current, int max)
    {
        currentLoop = current;
        maxLoop = max;
        UpdateLoopText();
    }

    void UpdateLoopText()
    {
        if (loopText != null)
            loopText.text = $"루프 {currentLoop} / {maxLoop}"; // 🔧 한글도 다시 확인
    }

    // ✅ 보상 텍스트 업데이트 함수
    public void ShowRewardSummary(string rewardSummary)
    {
        if (rewardText != null)
            rewardText.text = rewardSummary;
    }

    // ✅ 보상 텍스트 초기화 함수
    public void ClearRewardText()
    {
        if (rewardText != null)
            rewardText.text = "";
    }
}
