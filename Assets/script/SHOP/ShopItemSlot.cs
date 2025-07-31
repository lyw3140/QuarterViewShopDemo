using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    [Header("UI 연결")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Button buyButton;
    public TextMeshProUGUI buyButtonText;

    private ShopItemData itemData;
    private ShopUIManager shopManager;

    public void Setup(ShopItemData data, ShopUIManager manager)
    {
        itemData = data;
        shopManager = manager;

        iconImage.sprite = data.icon;
        nameText.text = data.itemName;
        priceText.text = $"{data.price}G";

        UpdateState();

        buyButton.onClick.RemoveAllListeners(); // 중복 방지
        buyButton.onClick.AddListener(OnBuy);
    }

    private void OnBuy()
    {
        shopManager.TryBuyItem(itemData, this);
    }

    public void UpdateState()
    {
        if (itemData.isLimited && itemData.hasPurchased)
        {
            buyButton.interactable = false;
            buyButtonText.text = "구매 완료";
        }
        else
        {
            buyButton.interactable = true;
            buyButtonText.text = "구매";
        }
    }
}
