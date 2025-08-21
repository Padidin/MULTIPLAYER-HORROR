using UnityEngine;
using Photon.Pun;

public class InventoryManagerBase : MonoBehaviourPun
{
    public InventoryItem[] items = new InventoryItem[6];
    public InventorySlotUI[] uiSlots;
    public Transform playerHandTransform;
    private GameObject heldItemInstance;
    private int currentHeldIndex = -1;

    void Update()
    {
        // Ganti slot (Alpha1 = slot 0, Alpha2 = slot 1, dst.)
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ToggleItem(i);
            }
        }

        // Drop item
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropCurrentItem();
        }
    }

    public bool AddItem(InventoryItem item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                if (uiSlots[i] != null)
                    uiSlots[i].SetIcon(item.icon);
                return true;
            }
        }
        return false;
    }

    void ToggleItem(int index)
    {
        if (items[index] == null)
            return;

        // Kalau slot yang sama → unequip
        if (currentHeldIndex == index)
        {
            UnequipItem();
        }
        else
        {
            UnequipItem(); // lepas dulu item sebelumnya

            // Spawn item di tangan
            heldItemInstance = Instantiate(items[index].prefab, playerHandTransform);
            heldItemInstance.transform.localPosition = Vector3.zero;
            heldItemInstance.transform.localRotation = Quaternion.identity;

            Rigidbody rb = heldItemInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            currentHeldIndex = index;
            HighlightSlot(index);
        }
    }

    public void DropCurrentItem()
    {
        if (currentHeldIndex == -1 || items[currentHeldIndex] == null)
            return;

        // Spawn item di dunia (Photon)
        GameObject droppedItem = PhotonNetwork.Instantiate(
            items[currentHeldIndex].prefab.name,
            playerHandTransform.position + playerHandTransform.forward * 0.5f,
            Quaternion.identity
        );

        // Kasih physics
        Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.AddForce(playerHandTransform.forward * 2f, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
        }

        // Bersihkan slot
        items[currentHeldIndex] = null;
        if (uiSlots[currentHeldIndex] != null)
            uiSlots[currentHeldIndex].Clear();

        Destroy(heldItemInstance);
        heldItemInstance = null;
        currentHeldIndex = -1;
        ResetAllSlotColors();
    }

    void UnequipItem()
    {
        if (heldItemInstance != null)
            Destroy(heldItemInstance);

        currentHeldIndex = -1;
        ResetAllSlotColors();
    }

    void HighlightSlot(int index)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (uiSlots[i] != null)
                uiSlots[i].Highlight(i == index);
        }
    }

    void ResetAllSlotColors()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (uiSlots[i] != null)
                uiSlots[i].Highlight(false);
        }
    }

    public bool HasEmptySlot()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
                return true;
        }
        return false;
    }

    public bool IsHoldingItem()
    {
        return currentHeldIndex != -1;
    }
}
