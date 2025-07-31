using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum ItemType
{
    None,
    Weapon,
    Armor,
    Potion,
    Key,
    Material,
    Backpack,
    Accessory
}

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemIcon;
    public TextMeshProUGUI countText;

    [Header("선택 시 테두리")]
    public GameObject selectorFrame;

    [Header("허용 아이템 타입")]
    public ItemType allowedSlotType = ItemType.None;

    private string itemID;
    private int itemCount;
    private CanvasGroup canvasGroup;

    void Start()
    {
        GetComponent<Button>()?.onClick.AddListener(OnSlotClicked);

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (selectorFrame != null)
            selectorFrame.SetActive(false); // 시작 시 테두리 숨기기
    }

    public void SetItem(string id, Sprite icon, int count = 1)
    {
        itemID = id;
        itemCount = count;
        itemIcon.sprite = icon;
        itemIcon.enabled = true;
        countText.text = itemCount > 1 ? itemCount.ToString() : "";
    }

    public void AddCount(int amount = 1)
    {
        itemCount += amount;
        countText.text = itemCount > 1 ? itemCount.ToString() : "";
    }

    public void Clear()
    {
        itemID = "";
        itemCount = 0;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        countText.text = "";

        if (selectorFrame != null)
            selectorFrame.SetActive(false);
    }

    public bool IsEmpty() => string.IsNullOrEmpty(itemID);
    public string GetItemID() => itemID;
    public int GetItemCount() => itemCount;

    private void OnSlotClicked()
    {
        if (!IsEmpty())
        {
            Debug.Log($"✅ 선택됨: {itemID}");
            InventorySystem.Instance.SelectSlot(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsEmpty()) return;

        canvasGroup.blocksRaycasts = false;

        if (DragItemUI.Instance != null)
        {
            DragItemUI.Instance.Show(itemIcon.sprite);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 슬롯 자체는 드래그하지 않음
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (DragItemUI.Instance != null)
        {
            DragItemUI.Instance.Hide();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObj = eventData.pointerDrag;
        if (draggedObj == null || draggedObj == gameObject) return;

        InventorySlot otherSlot = draggedObj.GetComponent<InventorySlot>();
        if (otherSlot == null || otherSlot == this) return;

        InventorySystem invSys = FindObjectOfType<InventorySystem>();
        var itemType = invSys.iconDB.FirstOrDefault(e => e.itemID == otherSlot.GetItemID())?.itemType ?? ItemType.None;

        if (allowedSlotType != ItemType.None && allowedSlotType != itemType)
        {
            Debug.Log($"❌ 이 슬롯은 {allowedSlotType} 타입만 장착할 수 없습니다. (현재 아이템: {itemType})");
            return;
        }

        string tempID = itemID;
        int tempCount = itemCount;
        Sprite tempIcon = itemIcon.sprite;

        SetItem(otherSlot.itemID, otherSlot.itemIcon.sprite, otherSlot.itemCount);
        otherSlot.SetItem(tempID, tempIcon, tempCount);

        invSys?.SwapSlots(this, otherSlot);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty())
        {
            string description = GetDescriptionByID(itemID);
            TooltipManager.Show(description, eventData.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Hide();
    }

    private string GetDescriptionByID(string id)
    {
        return id switch
        {
            "1" => "🔫 탄환: 기본 무기용 탄약입니다.",
            "12" => "🧪 회복약: 체력을 소량 회복합니다.",
            "123" => "💎 마법 수정: 스킬 강화에 사용됩니다.",
            _ => "아이템 정보 없음"
        };
    }
}
