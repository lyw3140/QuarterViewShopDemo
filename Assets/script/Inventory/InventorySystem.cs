using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    [Header("슬롯 및 아이콘 DB")]
    public List<InventorySlot> slots;

    [System.Serializable]
    public class InventoryIconEntry
    {
        public string itemID;
        public Sprite icon;
        public ItemType itemType;
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

    private InventorySlot selectedSlot;

    private void Awake()
    {
        Instance = this;
    }

    // ✅ 슬롯 선택 처리
    public void SelectSlot(InventorySlot slot)
    {
        if (selectedSlot != null && selectedSlot != slot)
            selectedSlot.selectorFrame?.SetActive(false);

        selectedSlot = slot;
        selectedSlot.selectorFrame?.SetActive(true);
    }

    // ✅ 슬롯 교환
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

    // ✅ 아이템 추가 (자동 슬롯 배정 기능 포함)
    public void AddItem(string id, Sprite icon, int amount = 1)
    {
        ItemType itemType = GetItemType(id);

        // 1단계: 타입 일치 + 동일 아이템 → 수량 추가
        foreach (var slot in slots)
        {
            if (slot == null) continue;
            if (slot.allowedSlotType != ItemType.None && slot.allowedSlotType != itemType) continue;

            if (!slot.IsEmpty() && slot.GetItemID() == id)
            {
                slot.AddCount(amount);
                return;
            }
        }

        // 2단계: 타입 일치 + 빈 슬롯 → 새로 배정
        foreach (var slot in slots)
        {
            if (slot == null) continue;
            if (slot.allowedSlotType != ItemType.None && slot.allowedSlotType != itemType) continue;

            if (slot.IsEmpty())
            {
                slot.SetItem(id, icon, amount);
                return;
            }
        }

        Debug.Log("❌ 인벤토리에 조건에 맞는 빈 슬롯이 없음");
    }

    // ✅ 인벤토리 저장
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

    // ✅ 인벤토리 불러오기
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
            var entry = iconDB.FirstOrDefault(e => e.itemID == data.itemID);
            if (entry != null)
                slots[i].SetItem(entry.itemID, entry.icon, data.count);
        }

        Debug.Log("📥 인벤토리 불러오기 완료");
    }

    // ✅ 아이템 타입 조회
    public ItemType GetItemType(string id)
    {
        return iconDB.FirstOrDefault(e => e.itemID == id)?.itemType ?? ItemType.None;
    }
}
