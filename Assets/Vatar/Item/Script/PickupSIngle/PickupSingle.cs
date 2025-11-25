using cakeslice;
using UnityEngine;
using UnityEngine.UI;

public class PickUpSingle : MonoBehaviour
{
    [Header("Player Setting")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private float pickUpDistance = 3f;
    public string namaPlayer;

    private ObjectGrab objectGrabbable;
    private ObjectGrab targetedObject;

    public GameObject logoPisau;
    public bool iniPisau;
    public bool iniPel;
    //public OutlineEraser outlineErase;

    void Update()
    {
        DetectGrabbableObject();

        if (iniPisau)
        {
            logoPisau.SetActive(true);
        }
        else
        {
            logoPisau.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null && targetedObject != null)
            {
                targetedObject.Grab(namaPlayer);
                objectGrabbable = targetedObject;
                targetedObject = null;
                if (objectGrabbable.namaBenda == "Pisau")
                {
                    iniPisau = true;
                }
                Debug.Log("Picked up object.");
                //InteractShow.instance.Hide();
            }
            else if (objectGrabbable != null)
            {
                objectGrabbable.Drop(namaPlayer);
                if (objectGrabbable.namaBenda == "Pisau")
                {
                    iniPisau = false;
                }

                objectGrabbable = null;
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

    /*private void OnTriggerEnter(Collider other)
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
    }*/
}