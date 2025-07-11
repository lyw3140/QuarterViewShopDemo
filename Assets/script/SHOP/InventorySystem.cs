using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("슬롯 및 아이콘 DB")]
    public List<InventorySlot> slots;

    [System.Serializable]
    public class InventoryIconEntry
    {
        public string itemID;
        public Sprite icon;
    }

    public List<InventoryIconEntry> iconDB;

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

    // ✅ 슬롯 순서도 반영 (드래그 후 리스트 정렬)
    public void SwapSlots(InventorySlot a, InventorySlot b)
    {
        int indexA = slots.IndexOf(a);
        int indexB = slots.IndexOf(b);

        if (indexA >= 0 && indexB >= 0)
        {
            slots[indexA] = b;
            slots[indexB] = a;
            Debug.Log($"🔁 슬롯 순서 교환: {indexA} <-> {indexB}");
        }
    }

    public void AddItem(string id, Sprite icon, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty() && slot.GetItemID() == id)
            {
                slot.AddCount(amount);
                return;
            }
        }

        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(id, icon, amount);
                return;
            }
        }

        Debug.Log("❌ 인벤토리 가득 참");
    }

    public void SaveInventory()
    {
        InventorySaveData saveData = new InventorySaveData();

        foreach (var slot in slots)
        {
            if (!slot.IsEmpty())
            {
                var data = new InventorySlotData
                {
                    itemID = slot.GetItemID(),
                    count = slot.GetItemCount()
                };
                saveData.slots.Add(data);
            }
        }

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("InventorySave", json);
        PlayerPrefs.Save();
        Debug.Log("💾 인벤토리 저장 완료");
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("InventorySave")) return;

        string json = PlayerPrefs.GetString("InventorySave");
        InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

        foreach (var slot in slots)
            slot.Clear();

        for (int i = 0; i < saveData.slots.Count && i < slots.Count; i++)
        {
            var data = saveData.slots[i];
            var icon = iconDB.FirstOrDefault(e => e.itemID == data.itemID)?.icon;
            if (icon != null)
                slots[i].SetItem(data.itemID, icon, data.count);
        }

        Debug.Log("📥 인벤토리 불러오기 완료");
    }
}
