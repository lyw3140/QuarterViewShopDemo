﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoopManager : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI loopText;
    public PortalUIManager portalUIManager;

    [Header("루프 수치")]
    public int currentLoop = 1;
    public int maxLoop = 10;

    [Header("자동 종료 설정")]
    public float loopDuration = 60f; // 한 루프 지속 시간 (초)
    private float timer;
    private bool isLoopActive = true;

    private void Start()
    {
        LoadLoopData(); // ✅ 루프 수 불러오기
        timer = 0f;
        UpdateLoopUI();
    }

    private void Update()
    {
        if (!isLoopActive) return;

        timer += Time.deltaTime;

        if (timer >= loopDuration)
        {
            EndLoop();
        }
    }

    void EndLoop()
    {
        isLoopActive = false;

        if (currentLoop < maxLoop)
        {
            currentLoop++;
            Debug.Log($"🌀 루프 자동 종료 - 루프 수 증가: {currentLoop} / {maxLoop}");
        }
        else
        {
            Debug.Log("✅ 최대 루프에 도달했으므로 종료됨");
        }

        SaveLoopData();
        SceneManager.LoadScene("BaseScene"); // 실제 기지 씬 이름
    }

    public void GoToNextLoop()
    {
        if (currentLoop < maxLoop)
        {
            currentLoop++;
            UpdateLoopUI();
            Debug.Log($"🌀 수동 루프 증가: {currentLoop} / {maxLoop}");

            if (portalUIManager != null)
                portalUIManager.ShowPortalUI();
            else
                Debug.LogWarning("⚠ PortalUIManager가 연결되지 않았습니다.");

            SaveLoopData();
        }
        else
        {
            Debug.Log("❌ 최대 루프 도달");
        }
    }

    void UpdateLoopUI()
    {
        if (loopText != null)
            loopText.text = $"🌀 {currentLoop} / {maxLoop}";
    }

    void SaveLoopData()
    {
        PlayerPrefs.SetInt("LoopCount", currentLoop);
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        PlayerGoldManager.Instance?.SaveGold(); // ✅ 골드 저장 추가
        PlayerPrefs.Save();
        Debug.Log("💾 루프 및 골드 저장 완료");
    }

    void LoadLoopData()
    {
        currentLoop = PlayerPrefs.GetInt("LoopCount", 1); // 기본값 1
        PlayerGoldManager.Instance?.LoadGold(); // ✅ (선택) 골드 불러오기
    }
}
