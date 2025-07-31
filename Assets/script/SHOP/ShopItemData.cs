using UnityEngine; // ✅ Sprite와 관련된 Unity 네임스페이스

[System.Serializable]
public class ShopItemData
{
    public string itemName;
    public int price;
    public Sprite icon;
    public bool isLimited;
    [HideInInspector] public bool hasPurchased;
}
