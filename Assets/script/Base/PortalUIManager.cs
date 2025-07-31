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

    // 이동할 씬 이름들
    private string[] sceneNames = { "VillageMap", "Desert_demoscene", "EpicWoodland_Winter_Free" };
    private string[] sceneDisplayNames = { "마을", "사막", "겨울숲" };

    void Start()
    {
        int buttonCount = Mathf.Min(portalButtons.Length, sceneNames.Length);

        for (int i = 0; i < buttonCount; i++)
        {
            int index = i;
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
        UpdateButtonColors();
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

    void UpdateButtonColors()
    {
        for (int i = 0; i < portalButtons.Length; i++)
        {
            var colors = portalButtons[i].colors;
            colors.normalColor = (i == selectedIndex) ? Color.green : Color.white;
            portalButtons[i].colors = colors;
        }
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
        SceneManager.LoadScene(targetScene);
    }
}
