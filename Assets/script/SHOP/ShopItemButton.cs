
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
    }

    void OnBuy()
    {
        if (goldManager.CurrentGold >= price)
        {
            goldManager.SpendGold(price);
            inventory.AddItem(itemID, itemIcon); // 인벤토리에 아이템 추가
            Debug.Log($"✅ {itemID} 구입 완료!");
        }
        else
        {
            Debug.Log("❌ 골드 부족!");
        }
    }
}
