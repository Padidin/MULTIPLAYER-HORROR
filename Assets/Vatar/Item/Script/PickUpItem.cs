using Photon.Pun;
using UnityEngine;

public class PickupItem : MonoBehaviourPun
{
    public InventoryItem itemData;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (InventoryManager.Instance.HasEmptySlot())
            {
                photonView.RPC("OnPickedUp", RpcTarget.AllBuffered);
                InventoryManager.Instance.AddItem(itemData);
                UIManager.Instance.ShowInteractText(false);
            }
            else
            {
                UIManager.Instance.ShowNotification("Inventory Penuh");
            }
        }
    }

    [PunRPC]
    void OnPickedUp()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            playerInRange = true;

            if (!InventoryManager.Instance.IsHoldingItem())
            {
                UIManager.Instance.ShowInteractText(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
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
