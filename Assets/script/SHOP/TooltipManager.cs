using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;

    private static TooltipManager instance;

    void Awake()
    {
        instance = this;
        Hide();  // ✅ 함수명 수정됨
    }

    public static void Show(string text, Vector2 position)
    {
        if (instance == null) return;

        instance.tooltipText.text = text;
        instance.tooltipPanel.SetActive(true);
        instance.tooltipPanel.transform.position = position;
    }

    public static void Hide()
    {
        if (instance == null) return;

        instance.tooltipPanel.SetActive(false);
    }
}
