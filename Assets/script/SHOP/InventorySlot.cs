using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemIcon;
    public TextMeshProUGUI countText;

    private string itemID;
    private int itemCount;

    private Vector2 originalPosition;
    private CanvasGroup canvasGroup;

    void Start()
    {
        GetComponent<Button>()?.onClick.AddListener(OnSlotClicked);

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    // ✅ 아이템 설정
    public void SetItem(string id, Sprite icon, int count = 1)
    {
        itemID = id;
        itemCount = count;
        itemIcon.sprite = icon;
        itemIcon.enabled = true;
        countText.text = itemCount > 1 ? itemCount.ToString() : "";
    }

    // ✅ 수량 추가
    public void AddCount(int amount = 1)
    {
        itemCount += amount;
        countText.text = itemCount > 1 ? itemCount.ToString() : "";
    }

    // ✅ 비우기
    public void Clear()
    {
        itemID = "";
        itemCount = 0;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        countText.text = "";
    }

    // ✅ Getter
    public bool IsEmpty() => string.IsNullOrEmpty(itemID);
    public string GetItemID() => itemID;
    public int GetItemCount() => itemCount;

    // ✅ 클릭 → 버리기
    private void OnSlotClicked()
    {
        if (!IsEmpty())
        {
            Debug.Log($"❌ {itemID} 아이템 버림");
            Clear();
        }
    }

    // ✅ 드래그 앤 드롭
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = originalPosition;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObj = eventData.pointerDrag;
        if (draggedObj == null || draggedObj == gameObject) return;

        InventorySlot otherSlot = draggedObj.GetComponent<InventorySlot>();
        if (otherSlot == null || otherSlot == this) return;

        // 스왑
        string tempID = itemID;
        int tempCount = itemCount;
        Sprite tempIcon = itemIcon.sprite;

        SetItem(otherSlot.itemID, otherSlot.itemIcon.sprite, otherSlot.itemCount);
        otherSlot.SetItem(tempID, tempIcon, tempCount);

        // ✅ 리스트 순서까지 반영
        FindObjectOfType<InventorySystem>()?.SwapSlots(this, otherSlot);
    }

    // ✅ 툴팁 표시
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
        // 🧠 예시로 간단하게 작성. 나중엔 DB나 딕셔너리로 분리 가능
        switch (id)
        {
            case "1": return "🔫 탄환: 기본 무기용 탄약입니다.";
            case "12": return "🧪 회복약: 체력을 소량 회복합니다.";
            case "123": return "💎 마법 수정: 스킬 강화에 사용됩니다.";
            default: return "아이템 정보 없음";
        }
    }
}
