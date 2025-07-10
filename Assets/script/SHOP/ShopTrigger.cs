using UnityEngine;
using UnityEngine.InputSystem;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI;
    private bool isPlayerInRange = false;
    private BasicPlayerMovement playerMovement;
    private bool isShopOpen = false;

    [SerializeField] private PlayerInput playerInput;

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
        SetCursorState(true); // 마우스 보이게

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

        // ✅ 마우스를 숨기지 않고 유지
        SetCursorState(true); // 무조건 보이도록 유지

        if (playerMovement != null)
            playerMovement.canMove = true;

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("Player");

        isShopOpen = false;
        Debug.Log("🔴 상점 닫힘 (마우스 유지)");
    }

    void SetCursorState(bool isVisible)
    {
        Cursor.visible = true; // ✅ 항상 true
        Cursor.lockState = CursorLockMode.None; // ✅ 항상 None (움직일 수 있게)
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
            CloseShop();  // 나가도 마우스는 유지됨
            Debug.Log("⛔ 상점 범위 이탈 - UI 닫힘");
        }
    }
}
