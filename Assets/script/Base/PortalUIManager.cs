using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalUIManager : MonoBehaviour
{
    [Header("UI 연결")]
    public Button[] portalButtons;
    public Button confirmButton;
    public TextMeshProUGUI titleText;

    private int selectedIndex = -1;

    // 실제 이동할 씬 이름들
    private string[] sceneNames = { "VillageMap", "Map_Forest", "Map_Cave" };

    // 플레이어에게 보여질 포탈 이름
    private string[] sceneDisplayNames = { "마을", "숲", "동굴" };

    void Start()
    {
        for (int i = 0; i < portalButtons.Length; i++)
        {
            int index = i; // 클로저 방지
            portalButtons[i].onClick.AddListener(() => SelectPortal(index));
        }

        confirmButton.onClick.AddListener(OnConfirm);
        HidePortalUI();
    }

    public void ShowPortalUI()
    {
        gameObject.SetActive(true);
        titleText.text = "이동할 포탈을 선택하세요!";
    }

    public void HidePortalUI()
    {
        gameObject.SetActive(false);
        selectedIndex = -1;
        ResetButtonColors();
    }

    void SelectPortal(int index)
    {
        if (index < 0 || index >= sceneNames.Length)
        {
            Debug.LogWarning("❌ 잘못된 포탈 인덱스");
            return;
        }

        selectedIndex = index;
        ResetButtonColors();
        HighlightButton(index);
        titleText.text = $"\uD83C\uDF00 선택된 포탈: {sceneDisplayNames[index]}";
    }

    void ResetButtonColors()
    {
        foreach (var btn in portalButtons)
        {
            var colors = btn.colors;
            colors.normalColor = Color.white;
            btn.colors = colors;
        }
    }

    void HighlightButton(int index)
    {
        var colors = portalButtons[index].colors;
        colors.normalColor = Color.green;
        portalButtons[index].colors = colors;
    }

    void OnConfirm()
    {
        if (selectedIndex == -1)
        {
            Debug.Log("❗ 포탈을 선택하세요.");
            return;
        }

        string targetScene = sceneNames[selectedIndex];

        Debug.Log($"✅ '{sceneDisplayNames[selectedIndex]}' 맵으로 이동 중... ({targetScene})");

        HidePortalUI();
        SceneManager.LoadScene(targetScene); // 여기서 직접 로딩함
    }
}
