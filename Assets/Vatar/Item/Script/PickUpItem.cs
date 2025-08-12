using Photon.Pun;
using UnityEngine;

public class PickupItem : MonoBehaviourPun
{
    public InventoryItem itemData;

    private InventoryManagerBase inventoryManager;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (inventoryManager != null && inventoryManager.HasEmptySlot())
            {
                photonView.RPC("RPC_RequestDestroy", RpcTarget.MasterClient);

                inventoryManager.AddItem(itemData);

                UIManager.Instance.ShowInteractText(false);
            }
            else
            {
                UIManager.Instance.ShowNotification("Inventory Penuh");
            }
        }
    }

    [PunRPC]
    void RPC_RequestDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        PhotonView pv = other.GetComponent<PhotonView>();

        if (pv != null && pv.IsMine)
        {
            if (other.CompareTag("Argha"))
            {
                inventoryManager = GameObject.FindGameObjectWithTag("ArghaInventory")?.GetComponent<InventoryManagerBase>();
            }
            else if (other.CompareTag("Irul"))
            {
                inventoryManager = GameObject.FindGameObjectWithTag("IrulInventory")?.GetComponent<InventoryManagerBase>();
            }

            playerInRange = true;

            if (inventoryManager != null && !inventoryManager.IsHoldingItem())
            {
                UIManager.Instance.ShowInteractText(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PhotonView pv = other.GetComponent<PhotonView>();

        if (pv != null && pv.IsMine)
        {
            playerInRange = false;
            UIManager.Instance.ShowInteractText(false);
            inventoryManager = null;
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
