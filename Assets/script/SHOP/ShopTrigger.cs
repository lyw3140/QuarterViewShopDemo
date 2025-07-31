using UnityEngine;
using UnityEngine.InputSystem;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI;
    private bool isPlayerInRange = false;
    private BasicPlayerMovement playerMovement;
    private bool isShopOpen = false;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private ShopUIManager shopUIManager; // ✅ 추가된 부분

    void Update()
    {
        if (isPlayerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!isShopOpen)
                OpenShop();
            else
                CloseShop();
        }
    }

    void OpenShop()
    {
        shopUI.SetActive(true);
        SetCursorState(true);

        if (shopUIManager != null) // ✅ ShopUIManager 통해 슬롯 생성
            shopUIManager.OpenShop();

        if (playerMovement != null)
            playerMovement.canMove = false;

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("UI");

        isShopOpen = true;
        Debug.Log("🟢 상점 열림");
    }

    void CloseShop()
    {
        shopUI.SetActive(false);
        SetCursorState(true);

        if (playerMovement != null)
            playerMovement.canMove = true;

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("Player");

        isShopOpen = false;
        Debug.Log("🔴 상점 닫힘 (마우스 유지)");
    }

    void SetCursorState(bool isVisible)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerMovement = other.GetComponent<BasicPlayerMovement>();
            Debug.Log("🛒 상점 범위 진입");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            CloseShop();
            Debug.Log("⛔ 상점 범위 이탈 - UI 닫힘");
        }
    }
}
