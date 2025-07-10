using UnityEngine;
using System.Collections.Generic; // ✅ 꼭 필요함!

[System.Serializable]
public class InventorySlotData
{
    public string itemID;
    public int count;
}

[System.Serializable]
public class InventorySaveData
{
    public List<InventorySlotData> slots = new List<InventorySlotData>();
}
