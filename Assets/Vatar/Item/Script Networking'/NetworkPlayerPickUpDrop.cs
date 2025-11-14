using UnityEngine;
using Photon.Pun;
using cakeslice;

public class NetworkPlayerPickUpDrop : MonoBehaviourPun
{
    [Header("References")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private float pickUpDistance = 3f;
    public Transform objectGrabPointTransform;

    private ObjectGrabbable objectGrabbable;
    private ObjectGrabbable detectedObject;
    private ObjectGrabbable lastDetectedObject;
    void Update()
    {
        if (!photonView.IsMine) return;

        DetectObject();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null && detectedObject != null)
            {
                photonView.RPC("RPC_GrabObject", RpcTarget.All, detectedObject.photonView.ViewID);

                if (lastDetectedObject != null)
                {
                    lastDetectedObject.GarisHilang();
                }
            }
            else if (objectGrabbable != null)
            {
                Vector3 dropPosition = objectGrabPointTransform.position;
                photonView.RPC("RPC_DropObject", RpcTarget.All, dropPosition);
            }
        }
    }

    [PunRPC]
    private void RPC_GrabObject(int objectViewID)
    {
        PhotonView objectView = PhotonView.Find(objectViewID);
        if (objectView != null)
        {
            ObjectGrabbable grabbable = objectView.GetComponent<ObjectGrabbable>();
            if (grabbable != null && grabbable.CanBeGrabbed())
            {
                objectGrabbable = grabbable;

                objectGrabbable.photonView.RPC("NetworkGrab", RpcTarget.All,
                    photonView.ViewID,
                    objectGrabPointTransform.position,
                    objectGrabPointTransform.rotation);

                if (photonView.IsMine)
                {
                    InteractShow.instance.Hide();
                }
            }
        }
    }

    [PunRPC]
    private void RPC_DropObject(Vector3 dropPosition)
    {
        if (objectGrabbable != null)
        {
            objectGrabbable.photonView.RPC("NetworkDrop", RpcTarget.All, dropPosition);
            objectGrabbable = null;
        }
    }
    
    private void DetectObject()
    {
        if (objectGrabbable != null)
        {
            ClearLastDetectedObject();
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, pickUpDistance, pickUpLayerMask))
        {
            if (hit.transform.TryGetComponent(out ObjectGrabbable grabbable) && grabbable.CanBeGrabbed())
            {
                detectedObject = grabbable;

                if (detectedObject != lastDetectedObject) 
                {
                    if (lastDetectedObject != null)
                    {
                        lastDetectedObject.GarisHilang();
                    }

                    detectedObject.GarisMuncul();
                    lastDetectedObject = detectedObject;
                }
                
                InteractShow.instance.Show();
                return;
            }
        }

        ClearLastDetectedObject();
    }

    private void ClearLastDetectedObject()
    {
        if (lastDetectedObject != null)
        {
            lastDetectedObject.GarisHilang();
            lastDetectedObject = null;
        }

        detectedObject = null;
        InteractShow.instance.Hide();
    }
}