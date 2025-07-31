using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject inventoryCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryCanvas != null)
            {
                inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
            }
        }
    }
}
