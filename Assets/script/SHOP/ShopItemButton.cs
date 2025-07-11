using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    public string itemID;
    public Sprite itemIcon;
    public int price = 100;

    private Button button;
    private PlayerGoldManager goldManager;
    private InventorySystem inventory;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnBuy);

        goldManager = FindObjectOfType<PlayerGoldManager>();
        inventory = FindObjectOfType<InventorySystem>();

        if (goldManager == null)
        {
            Debug.LogError("❌ goldManager가 null입니다!");
        }

        if (inventory == null)
        {
            Debug.LogError("❌ inventory가 null입니다!");
        }

        // itemID가 유효할 때만 아이콘 자동 설정
        if (!string.IsNullOrEmpty(itemID) && inventory != null)
        {
            var match = inventory.iconDB.Find(e => e.itemID == itemID);
            if (match != null && match.icon != null)
            {
                itemIcon = match.icon;
                Debug.Log($"🎯 아이콘 자동 설정 완료: {itemID}");
            }
            else
            {
                Debug.LogWarning($"⚠️ itemID '{itemID}'에 해당하는 아이콘을 iconDB에서 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ itemID가 비어 있거나 InventorySystem이 없습니다.");
        }
    }

    void OnBuy()
    {
        if (goldManager == null || inventory == null)
        {
            Debug.LogError("❌ goldManager 또는 inventory가 null입니다!");
            return;
        }

        if (string.IsNullOrEmpty(itemID))
        {
            Debug.LogError("❌ itemID가 비어있습니다!");
            return;
        }

        // OnBuy 시에도 itemIcon이 null이면 다시 시도
        if (itemIcon == null)
        {
            var match = inventory.iconDB.Find(e => e.itemID == itemID);
            if (match != null && match.icon != null)
            {
                itemIcon = match.icon;
                Debug.Log($"🔄 OnBuy 시점에서 아이콘 재설정: {itemID}");
            }
        }

        if (itemIcon == null)
        {
            Debug.LogError($"❌ itemIcon이 여전히 null입니다! itemID: {itemID}");
            return;
        }

        if (goldManager.CurrentGold >= price)
        {
            goldManager.SpendGold(price);
            inventory.AddItem(itemID, itemIcon);
            Debug.Log($"✅ {itemID} 구입 완료!");
        }
        else
        {
            Debug.Log("❌ 골드 부족!");
        }
    }
}
