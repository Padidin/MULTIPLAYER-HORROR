using cakeslice;
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null && targetedObject != null)
            {
                targetedObject.Grab(objectGrabPointTransform);
                objectGrabbable = targetedObject;
                targetedObject = null;
                Debug.Log("Picked up object.");
                //InteractShow.instance.Hide();
            }
            else if (objectGrabbable != null)
            {
                Vector3 dropPosition = objectGrabPointTransform.position;

                objectGrabbable.Drop();


                objectGrabbable = null;

                Debug.Log("Dropped object at: " + dropPosition);
                //InteractShow.instance.Hide();
            }

        }
    }

    private void DetectGrabbableObject()
    {

        if (objectGrabbable != null)
        {
            return;
        }

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, pickUpDistance, pickUpLayerMask))
        {
            if (hit.transform.TryGetComponent(out ObjectGrab grabbable))
            {
                if (targetedObject != grabbable)
                {
                    if (targetedObject != null)
                    {
                        targetedObject.OutlineHilang();
                    }

                    targetedObject = grabbable;
                    targetedObject.OutlineMuncul();

                   /* if (currentOutline != null)
                    {
                        currentOutline.eraseRenderer = true;
                    }

                    if (hit.transform.TryGetComponent(out Outline outlineBaru))
                    {
                        currentOutline = outlineBaru;
                        currentOutline.eraseRenderer = false;
                    }*/

                }
                return;
            }
        }

        if (targetedObject != null)
        {
            targetedObject.OutlineHilang();
            targetedObject = null;
        }

        targetedObject = null;
    }

    /*public void OnPickupButtonPressed()
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
    }*/

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