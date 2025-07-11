using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject interactionHintUI;
    public GameObject targetUIPanel;

    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (targetUIPanel != null)
                targetUIPanel.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionHintUI?.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionHintUI?.SetActive(false);
        }
    }
}
