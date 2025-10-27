using UnityEngine;

public class PickUpSingle : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private float pickUpDistance = 3f;

    private ObjectGrab objectGrabbable;
    private ObjectGrab targetedObject;
    //public OutlineEraser outlineErase;

    void Update()
    {
        DetectGrabbableObject();
    }

    private void DetectGrabbableObject()
    {
        targetedObject = null;

        if (objectGrabbable != null)
        {
            return;
        }

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, pickUpDistance, pickUpLayerMask))
        {
            if (hit.transform.TryGetComponent(out ObjectGrab grabbable))
            {
                targetedObject = grabbable;
                return;
            }
        }

    }

    public void OnPickupButtonPressed()
    {
        if (objectGrabbable == null && targetedObject != null)
        {
            targetedObject.Grab(objectGrabPointTransform);
            objectGrabbable = targetedObject;
        }
        else if (objectGrabbable != null)
        {
            objectGrabbable.Drop();
            objectGrabbable = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            //outlineErase.OutlineMuncul(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            //outlineErase.OutlineHilang(other.gameObject);
        }
    }
}