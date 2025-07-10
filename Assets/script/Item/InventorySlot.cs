
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image itemIcon;
    public TextMeshProUGUI countText;

    private string itemID;
    private int itemCount;

    private Vector2 originalPosition;
    private CanvasGroup canvasGroup;

    void Start()
    {
        // 슬롯 클릭 시 아이템 버리기
        GetComponent<Button>()?.onClick.AddListener(OnSlotClicked);

        // 드래그 처리를 위한 CanvasGroup 준비
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

    // ✅ 아이템 수량 추가
    public void AddCount(int amount = 1)
    {
        itemCount += amount;
        countText.text = itemCount > 1 ? itemCount.ToString() : "";
    }

    // ✅ 아이템 초기화 (비우기)
    public void Clear()
    {
        itemID = "";
        itemCount = 0;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        countText.text = "";
    }

    // ✅ 현재 슬롯이 비어있는지 확인
    public bool IsEmpty() => string.IsNullOrEmpty(itemID);

    // ✅ 외부 접근용 Getter
    public string GetItemID() => itemID;
    public int GetItemCount() => itemCount;

    // ✅ 슬롯 클릭 시 아이템 버리기
    private void OnSlotClicked()
    {
        if (!IsEmpty())
        {
            Debug.Log($"❌ {itemID} 아이템 버림");
            Clear();
        }
    }

    // 🔁 드래그 앤 드롭 (순서 바꾸기)
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

        // 스왑 처리
        string tempID = itemID;
        int tempCount = itemCount;
        Sprite tempIcon = itemIcon.sprite;

        SetItem(otherSlot.itemID, otherSlot.itemIcon.sprite, otherSlot.itemCount);
        otherSlot.SetItem(tempID, tempIcon, tempCount);
    }
}
