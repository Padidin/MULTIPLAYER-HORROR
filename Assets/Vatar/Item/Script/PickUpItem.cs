using Photon.Pun;
using UnityEngine;

public class PickupItem : MonoBehaviourPun
{
    public InventoryItem itemData;

    private InventoryManagerBase inventoryManager;
    private bool playerInRange = false;

    void Update()
    {
        if (!playerInRange || inventoryManager == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventoryManager.HasEmptySlot())
            {
                inventoryManager.AddItem(itemData);

                photonView.RPC("RPC_RequestDestroy", RpcTarget.MasterClient);

                UIManager.Instance.ShowInteractText(false);
                playerInRange = false;
            }
            else
            {
                UIManager.Instance.ShowNotification("Inventory penuh");
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

        if (playerPV != null && playerPV.IsMine)
        {
            if (other.CompareTag("Argha"))
            {
                GameObject go = GameObject.FindGameObjectWithTag("ArghaInventory");
                if (go != null)
                    inventoryManager = go.GetComponent<InventoryManagerBase>();
            }
            else if (other.CompareTag("Irul"))
            {
                GameObject go = GameObject.FindGameObjectWithTag("IrulInventory");
                if (go != null)
                    inventoryManager = go.GetComponent<InventoryManagerBase>();
            }

            if (inventoryManager != null && !inventoryManager.IsHoldingItem())
            {
                UIManager.Instance.ShowInteractText(true);
            }
            else
            {
                Debug.LogWarning("InventoryManager tidak ditemukan untuk player ini!");
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
