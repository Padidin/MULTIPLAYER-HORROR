using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventoryItem[] items = new InventoryItem[6];
    public InventorySlotUI[] uiSlots;
    public Transform playerHandTransform;
    private GameObject heldItemInstance;
    private int currentHeldIndex = -1;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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

    void DropCurrentItem()
    {
        if (currentHeldIndex == -1 || items[currentHeldIndex] == null)
            return;

        Vector3 dropPos = playerHandTransform.position + playerHandTransform.forward * 1.5f;
        GameObject dropped = Instantiate(items[currentHeldIndex].prefab, dropPos, Quaternion.identity);

        Rigidbody rb = dropped.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(playerHandTransform.forward * 0.3f + Vector3.up * 2f, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * 200f, ForceMode.Impulse);
        }

        items[currentHeldIndex] = null;
        uiSlots[currentHeldIndex].Clear();
        UnequipItem();
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
        if (currentHeldIndex != -1)
        {
            items[currentHeldIndex] = newItem;

            if (uiSlots[currentHeldIndex] != null)
                uiSlots[currentHeldIndex].SetIcon(newItem.icon);

            if (heldItemInstance != null)
                Destroy(heldItemInstance);

            heldItemInstance = Instantiate(newItem.prefab, playerHandTransform);
            heldItemInstance.transform.localPosition = Vector3.zero;
            heldItemInstance.transform.localRotation = Quaternion.identity;

            Rigidbody rb = heldItemInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            HighlightSlot(currentHeldIndex);
        }
        else
        {
            AddItem(newItem); 
        }
    }


}
