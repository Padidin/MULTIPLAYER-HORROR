using UnityEngine;
using Photon.Pun;
using cakeslice; // <-- Pastikan using ini ada

public class NetworkPlayerPickUpDrop : MonoBehaviourPun
{
    [Header("References")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private float pickUpDistance = 3f;
    public Transform objectGrabPointTransform;

    private ObjectGrabbable objectGrabbable;
    private ObjectGrabbable detectedObject;
    private ObjectGrabbable lastDetectedObject; // <-- Variabel ini sudah ada, bagus!

    void Update()
    {
        // Hanya player lokal yang bisa mendeteksi & berinteraksi
        if (!photonView.IsMine) return;

        DetectObject();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null && detectedObject != null)
            {
                // Ambil objek
                photonView.RPC("RPC_GrabObject", RpcTarget.All, detectedObject.photonView.ViewID);

                // Langsung matikan outline setelah menekan tombol E
                if (lastDetectedObject != null)
                {
                    lastDetectedObject.GetComponent<Outline>().enabled = false;
                    lastDetectedObject = null;
                }
            }
            else if (objectGrabbable != null)
            {
                // Jatuhkan objek
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

                // Kirim RPC ke objek untuk diambil
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

            // UI di-handle oleh DetectObject() di frame berikutnya
        }
    }
    
    // --- FUNGSI DETEKSI YANG DIPERBAIKI ---
    private void DetectObject()
    {
        // Jika sedang membawa objek, jangan deteksi outline apa pun
        if (objectGrabbable != null)
        {
            ClearLastDetectedObject(); // Pastikan tidak ada outline yang menyala
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, pickUpDistance, pickUpLayerMask))
        {
            if (hit.transform.TryGetComponent(out ObjectGrabbable grabbable) && grabbable.CanBeGrabbed())
            {
                detectedObject = grabbable;

                // Cek jika kita melihat objek baru
                if (detectedObject != lastDetectedObject) 
                {
                    // Matikan outline di objek lama (jika ada)
                    if (lastDetectedObject != null)
                    {
                        lastDetectedObject.GetComponent<Outline>().enabled = false;
                    }

                    // Nyalakan outline di objek baru
                    detectedObject.GetComponent<Outline>().enabled = true;
                    lastDetectedObject = detectedObject;
                }
                
                InteractShow.instance.Show();
                return;
            }
        }

        // Jika raycast tidak mengenai objek yang bisa diambil
        ClearLastDetectedObject();
    }

    // --- FUNGSI BANTUAN BARU ---
    private void ClearLastDetectedObject()
    {
        if (lastDetectedObject != null)
        {
            // Matikan outline di objek terakhir yang kita lihat
            lastDetectedObject.GetComponent<Outline>().enabled = false;
            lastDetectedObject = null;
        }

        detectedObject = null;
        InteractShow.instance.Hide();
    }
}