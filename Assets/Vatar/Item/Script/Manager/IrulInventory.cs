using UnityEngine;

public class IrulInventory : InventoryManagerBase
{
    private void Update()
    {
        if (playerHandTransform == null)
        {
            GameObject IrulGrab = GameObject.FindGameObjectWithTag("GrabIrul");
            if (IrulGrab != null)
            {
                playerHandTransform = IrulGrab.transform;
            }
        }
    }

}
