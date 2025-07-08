using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItem itemData;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (InventoryManager.Instance.AddItem(itemData))
            {
                UIManager.Instance.ShowInteractText(false);
                Destroy(gameObject);
            }
            else
            {
                UIManager.Instance.ShowNotification("Inventory Penuh");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            UIManager.Instance.ShowInteractText(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            UIManager.Instance.ShowInteractText(false);
        }
    }

    private void OnDisable()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowInteractText(false);
        }
    }
}
