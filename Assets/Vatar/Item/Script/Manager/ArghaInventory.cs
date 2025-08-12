using UnityEngine;

public class ArghaInventory : InventoryManagerBase
{
    private void Update()
    {
        if (playerHandTransform == null)
        {
            GameObject arghaGrab = GameObject.FindGameObjectWithTag("GrabArgha");
            if (arghaGrab != null)
            {
                playerHandTransform = arghaGrab.transform;
            }
        }
    }

}
