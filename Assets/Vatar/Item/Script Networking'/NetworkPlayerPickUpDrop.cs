using UnityEngine;
using Photon.Pun;

public class NetworkPlayerPickUpDrop : MonoBehaviourPun
{
    [Header("References")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private float pickUpDistance = 3f;
    public Transform objectGrabPointTransform;

    private ObjectGrabbable objectGrabbable;
    private ObjectGrabbable detectedObject;

    void Update()
    {
        if (!photonView.IsMine) return;

        DetectObject();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null && detectedObject != null)
            {
                // Grab object via network - KIRIM POSISI, BUKAN TRANSFORM
                photonView.RPC("RPC_GrabObject", RpcTarget.All, detectedObject.photonView.ViewID);
            }
            else if (objectGrabbable != null)
            {
                // Drop object via network
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

                // ✅ KIRIM DATA POSISI, BUKAN TRANSFORM
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

            if (photonView.IsMine)
            {
                InteractShow.instance.Hide();
            }
        }
    }

    private void DetectObject()
    {
        if (!photonView.IsMine) return;

        detectedObject = null;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, pickUpDistance, pickUpLayerMask))
        {
            if (hit.transform.TryGetComponent(out ObjectGrabbable grabbable))
            {
                if (objectGrabbable == null && grabbable.CanBeGrabbed())
                {
                    detectedObject = grabbable;
                    InteractShow.instance.Show();
                }
            }
        }
        else
        {
            InteractShow.instance.Hide();
        }
    }
}