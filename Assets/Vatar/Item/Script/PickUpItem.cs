using Photon.Pun;
using UnityEngine;

public class PickupItem : MonoBehaviourPun
{
    public InventoryItem itemData;

    private InventoryManagerBase inventoryManager;
    private bool playerInRange = false;

    void Update()
    {
        // Pickup hanya boleh dilakukan oleh player yang sedang dalam jangkauan
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (inventoryManager != null && inventoryManager.HasEmptySlot())
            {
                // Tambahkan ke inventory lokal player
                inventoryManager.AddItem(itemData);

                // Destroy item di semua client via MasterClient
                photonView.RPC("RPC_RequestDestroy", RpcTarget.MasterClient);

                UIManager.Instance.ShowInteractText(false);
                playerInRange = false;
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
        PhotonView playerPV = other.GetComponent<PhotonView>();

        // Pastikan ini player kita (local player) yang masuk trigger
        if (playerPV != null && playerPV.IsMine)
        {
            if (other.CompareTag("Argha"))
            {
                inventoryManager = GameObject.FindGameObjectWithTag("ArghaInventory")?.GetComponent<InventoryManagerBase>();
            }
            else if (other.CompareTag("Irul"))
            {
                inventoryManager = GameObject.FindGameObjectWithTag("IrulInventory")?.GetComponent<InventoryManagerBase>();
            }

            if (inventoryManager != null && !inventoryManager.IsHoldingItem())
            {
                UIManager.Instance.ShowInteractText(true);
            }

            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PhotonView playerPV = other.GetComponent<PhotonView>();

        if (playerPV != null && playerPV.IsMine)
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
