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
        Hide();
    }

    public static void Show(string text, Vector2 position)
    {
        if (instance == null) return;

        instance.tooltipText.text = text;
        instance.tooltipPanel.SetActive(true);

        // ✅ 깜빡임 방지를 위해 마우스에서 살짝 떨어진 위치로 설정
        Vector2 offset = new Vector2(20f, -20f);
        instance.tooltipPanel.transform.position = position + offset;
    }

    public static void Hide()
    {
        if (instance == null) return;

        instance.tooltipPanel.SetActive(false);
    }
}
