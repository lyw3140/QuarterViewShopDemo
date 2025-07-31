using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject shopUI;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI goldText;
    public GameObject itemSlotPrefab;
    public Transform itemListParent;

    [Header("Player Gold (테스트용)")]
    public int playerGold = 5000;

    [Header("상점 아이템")]
    public List<ShopItemData> shopItems = new List<ShopItemData>();

    // ✅ InventorySystem 참조
    private InventorySystem inventorySystem;

    void Start()
    {
        inventorySystem = InventorySystem.Instance; // 싱글톤 참조
        UpdateGoldUI();
    }

    void Update()
    {
        if (shopUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            shopUI.SetActive(false);
        }
    }

    public void OpenShop()
    {
        shopUI.SetActive(true);
        GenerateItemSlots();
        UpdateGoldUI();
    }

    private void GenerateItemSlots()
    {
        foreach (Transform child in itemListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in shopItems)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, itemListParent);
            ShopItemSlot slot = slotObj.GetComponent<ShopItemSlot>();
            slot.Setup(item, this);
        }
    }

    public void TryBuyItem(ShopItemData item, ShopItemSlot slot)
    {
        if (item.isLimited && item.hasPurchased)
        {
            ShowFeedback("❌ 이미 구매한 아이템입니다!");
            return;
        }

        if (playerGold >= item.price)
        {
            playerGold -= item.price;
            UpdateGoldUI();

            // ✅ 인벤토리 시스템에 아이템 추가
            if (inventorySystem != null)
                inventorySystem.AddItem(item.itemName, item.icon); // itemName은 ID 역할

            if (item.isLimited)
                item.hasPurchased = true;

            ShowFeedback($"✅ '{item.itemName}' 구매 완료!");
            slot.UpdateState();
        }
        else
        {
            ShowFeedback("❌ 잔액이 부족합니다!");
        }
    }

    public void ShowFeedback(string message)
    {
        if (feedbackText == null) return;

        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideFeedback));
        Invoke(nameof(HideFeedback), 1.5f);
    }

    private void HideFeedback()
    {
        feedbackText.gameObject.SetActive(false);
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
            goldText.text = $"Gold: {playerGold}";
    }
}

