using UnityEngine;

public class InventoryManagerBase : MonoBehaviour
{
    public InventoryItem[] items = new InventoryItem[6];
    public InventorySlotUI[] uiSlots;
    public Transform playerHandTransform;
    private GameObject heldItemInstance;
    private int currentHeldIndex = -1;

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

    void Update()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ToggleItem(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropCurrentItem();
        }
    }

    void ToggleItem(int index)
    {
        if (items[index] == null)
            return;

        if (currentHeldIndex == index)
        {
            UnequipItem();
        }
        else
        {
            UnequipItem();

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
        if (heldItemInstance != null && currentHeldIndex != -1)
        {
            GameObject droppedItem = Instantiate(items[currentHeldIndex].prefab, playerHandTransform.position + playerHandTransform.forward * 0.5f, Quaternion.identity);
            Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.interpolation = RigidbodyInterpolation.Interpolate;

                rb.AddForce(playerHandTransform.forward * 2f, ForceMode.Impulse);

                Vector3 randomTorque = new Vector3(
                    Random.Range(-5f, 5f),
                    Random.Range(-5f, 5f),
                    Random.Range(-5f, 5f)
                );
                rb.AddTorque(randomTorque, ForceMode.Impulse);
            }

            Destroy(heldItemInstance);
            heldItemInstance = null;
            items[currentHeldIndex] = null;
            uiSlots[currentHeldIndex].Clear();
            currentHeldIndex = -1;
        }
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

    public void ReplaceHeldItem(InventoryItem newItem)
    {
        if (IsHoldingItem())
        {
            AddItem(newItem);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = newItem;
                    if (uiSlots[i] != null)
                        uiSlots[i].SetIcon(newItem.icon);

                    heldItemInstance = Instantiate(newItem.prefab, playerHandTransform);
                    heldItemInstance.transform.localPosition = Vector3.zero;
                    heldItemInstance.transform.localRotation = Quaternion.identity;

                    Rigidbody rb = heldItemInstance.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.useGravity = false;
                    }

                    currentHeldIndex = i;
                    HighlightSlot(i);
                    break;
                }
            }
        }
    }
}
