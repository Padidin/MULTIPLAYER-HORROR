using UnityEngine;
using Photon.Pun;

public class InventoryManagerBase : MonoBehaviourPun
{
    [System.Serializable]
    public class InventorySlot
    {
        public InventoryItem item;     // Data item (icon, prefab reference)
        public string prefabName;      // Nama prefab untuk Photon.Instantiate
    }

    public InventorySlot[] slots = new InventorySlot[6];
    public InventorySlotUI[] uiSlots;
    public Transform playerHandTransform;
    private GameObject heldItemInstance;
    private int currentHeldIndex = -1;

    void Update()
    {
        // Ganti slot (Alpha1 = slot 0, Alpha2 = slot 1, dst.)
        for (int i = 0; i < slots.Length; i++)
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
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = item;
                slots[i].prefabName = item.prefab.name; // simpan nama prefab
                if (uiSlots[i] != null)
                    uiSlots[i].SetIcon(item.icon);
                return true;
            }
        }
        return false;
    }

    void ToggleItem(int index)
    {
        if (slots[index].item == null)
            return;

        // Kalau slot yang sama → unequip
        if (currentHeldIndex == index)
        {
            UnequipItem();
        }
        else
        {
            UnequipItem(); // lepas dulu item sebelumnya

            // Spawn item di tangan pakai Photon (agar sinkron)
            heldItemInstance = PhotonNetwork.Instantiate(
                slots[index].prefabName,
                playerHandTransform.position,
                playerHandTransform.rotation
            );

            // Parent ke tangan
            heldItemInstance.transform.SetParent(playerHandTransform);
            heldItemInstance.transform.localPosition = Vector3.zero;
            heldItemInstance.transform.localRotation = Quaternion.identity;

            // Disable physics saat di tangan
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
        if (currentHeldIndex == -1 || slots[currentHeldIndex].item == null)
            return;

        // Spawn item di dunia (Photon)
        GameObject droppedItem = PhotonNetwork.Instantiate(
            slots[currentHeldIndex].prefabName,
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
        slots[currentHeldIndex].item = null;
        slots[currentHeldIndex].prefabName = null;
        if (uiSlots[currentHeldIndex] != null)
            uiSlots[currentHeldIndex].Clear();

        if (heldItemInstance != null)
            PhotonNetwork.Destroy(heldItemInstance);

        heldItemInstance = null;
        currentHeldIndex = -1;
        ResetAllSlotColors();
    }

    void UnequipItem()
    {
        if (heldItemInstance != null)
            PhotonNetwork.Destroy(heldItemInstance);

        heldItemInstance = null;
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
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return true;
        }
        return false;
    }

    public bool IsHoldingItem()
    {
        return currentHeldIndex != -1;
    }
}
