using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> slots;

    // 아이템 아이콘 DB (간단 예시용)
    public List<InventoryIconEntry> iconDB;

    [System.Serializable]
    public class InventoryIconEntry
    {
        public string itemID;
        public Sprite icon;
    }

    // ✅ 인벤토리에 아이템 추가하는 함수
    public void AddItem(string id, Sprite icon, int amount = 1)
    {
        // 이미 있는 슬롯에 추가
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty() && slot.GetItemID() == id)
            {
                slot.AddCount(amount);
                return;
            }
        }

        // 비어 있는 슬롯에 새로 추가
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

    // 저장
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

    // 불러오기
    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("InventorySave")) return;

        string json = PlayerPrefs.GetString("InventorySave");
        InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

        // 슬롯 초기화
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
