using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    public GameObject portalUI;
    private bool isPlayerInRange = false;
    private BasicPlayerMovement playerMovement;
    private bool isPortalOpen = false;

    void Update()
    {
        if (isPlayerInRange)
        {
            // E 키로 열고 닫기
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isPortalOpen)
                    OpenPortal();
                else
                    ClosePortal();
            }

            // ESC 키로 닫기 (선택)
            if (isPortalOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePortal();
            }
        }
    }

    void OpenPortal()
    {
        portalUI.SetActive(true);
        if (playerMovement != null)
            playerMovement.canMove = false;

        isPortalOpen = true;
        Debug.Log("📍 포탈 UI 열림");
    }

    void ClosePortal()
    {
        portalUI.SetActive(false);
        if (playerMovement != null)
            playerMovement.canMove = true;

        isPortalOpen = false;
        Debug.Log("⬅ 포탈 UI 닫힘");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerMovement = other.GetComponent<BasicPlayerMovement>();
            Debug.Log("🌀 플레이어 포탈 범위 진입");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            ClosePortal(); // 범위 이탈 시 자동 닫기
            Debug.Log("⬅ 포탈 범위 이탈 - UI 자동 닫힘");
        }
    }
}
