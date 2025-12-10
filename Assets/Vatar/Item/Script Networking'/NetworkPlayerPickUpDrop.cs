using UnityEngine;
using Photon.Pun;
using cakeslice;

public class NetworkPlayerPickUpDrop : MonoBehaviourPun
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

    void Update()
    {
        if (!photonView.IsMine) return; // script ini hanya handle input & local raycast untuk player local

        DetectGrabbableObject();

        logoPisau.SetActive(iniPisau);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null && targetedObject != null)
            {
                // Request ownership and then broadcast grab
                PhotonView targetPV = targetedObject.GetComponent<PhotonView>();
                if (targetPV != null)
                {
                    // Request ownership first
                    targetPV.RequestOwnership();

                    // Send RPC to all to set grabbed state (buffered so late joiners know it's grabbed)
                    targetPV.RPC("RPC_Grab", RpcTarget.AllBuffered, photonView.ViewID, PhotonNetwork.LocalPlayer.ActorNumber);
                    // store locally
                    objectGrabbable = targetedObject;
                    targetedObject = null;

                    if (objectGrabbable.namaBenda == "Pisau") iniPisau = true;

                    Debug.Log("Picked up object (sent RPC).");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (objectGrabbable != null)
            {
                PhotonView pv = objectGrabbable.GetComponent<PhotonView>();
                if (pv != null)
                {
                    pv.RPC("RPC_Drop", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
                }

                if (objectGrabbable.namaBenda == "Pisau") iniPisau = false;

                objectGrabbable = null;
            }
        }
    }

    private void DetectGrabbableObject()
    {
        if (objectGrabbable != null) return;

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
}