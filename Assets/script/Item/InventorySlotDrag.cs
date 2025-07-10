using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector2 originalPosition;

    private InventorySlot slot;

    void Awake()
    {
        slot = GetComponent<InventorySlot>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
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
        var draggedObj = eventData.pointerDrag;
        if (draggedObj != null && draggedObj != gameObject)
        {
            var otherSlot = draggedObj.GetComponent<InventorySlot>();
            if (otherSlot != null && slot != null)
            {
                SwapItems(slot, otherSlot);
            }
        }
    }

    void SwapItems(InventorySlot a, InventorySlot b)
    {
        // 임시 저장
        string idA = a.GetItemID();
        Sprite iconA = a.GetComponent<InventorySlot>().itemIcon.sprite;
        int countA = a.GetComponent<InventorySlot>().itemIcon.enabled ? int.Parse(a.countText.text == "" ? "1" : a.countText.text) : 0;

        string idB = b.GetItemID();
        Sprite iconB = b.GetComponent<InventorySlot>().itemIcon.sprite;
        int countB = b.GetComponent<InventorySlot>().itemIcon.enabled ? int.Parse(b.countText.text == "" ? "1" : b.countText.text) : 0;

        // 스왑
        a.Clear();
        b.Clear();

        if (!string.IsNullOrEmpty(idB))
            a.SetItem(idB, iconB, countB);
        if (!string.IsNullOrEmpty(idA))
            b.SetItem(idA, iconA, countA);
    }
}
