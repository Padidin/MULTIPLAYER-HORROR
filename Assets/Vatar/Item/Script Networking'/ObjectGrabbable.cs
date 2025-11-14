using UnityEngine;
using Photon.Pun;
using cakeslice;

public class ObjectGrabbable : MonoBehaviourPun
{
    public string namaBenda;
    public Outline[] outlineObjek;
    public QuestPlayer questPlayer;

    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private Outline outline;
    private bool isBeingGrabbed = false;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();

        GameObject questObjek = GameObject.Find("Quest Object");
        questPlayer = questObjek.GetComponent<QuestPlayer>();
    }

    private void Start()
    {
        outline.enabled = false;
        outline.eraseRenderer = true;

        foreach (Outline garisObjek in  outlineObjek)
        {
            garisObjek.eraseRenderer = true;
        }
    }

    public void GarisMuncul()
    {
        foreach (Outline garisObjek in outlineObjek)
        {
            garisObjek.eraseRenderer = false;
        }
    }

    public void GarisHilang()
    {
        foreach (Outline garisObjek in outlineObjek)
        {
            garisObjek.eraseRenderer = true;
        }
    }

    // ✅ UBAH NAMA METHOD MENJADI "NetworkGrab"
    [PunRPC]
    public void NetworkGrab(int playerViewID, Vector3 grabPosition, Quaternion grabRotation)
    {
        if (isBeingGrabbed) return;

        PhotonView playerView = PhotonView.Find(playerViewID);
        if (playerView != null)
        {
            // ✅ CARI grabPointTransform DARI PLAYER
            NetworkPlayerPickUpDrop playerPickup = playerView.GetComponent<NetworkPlayerPickUpDrop>();
            if (playerPickup != null)
            {
                this.objectGrabPointTransform = playerPickup.objectGrabPointTransform;
            }
            else
            {
                // Fallback: buat temporary transform
                GameObject tempGrabPoint = new GameObject("TempGrabPoint");
                tempGrabPoint.transform.position = grabPosition;
                tempGrabPoint.transform.rotation = grabRotation;
                this.objectGrabPointTransform = tempGrabPoint.transform;
            }

            objectRigidbody.useGravity = false;
            objectRigidbody.isKinematic = true;
            isBeingGrabbed = true;

            questPlayer.GrabObject(namaBenda);

            // ✅ TRANSFER OWNERSHIP YANG BENAR
            photonView.RequestOwnership();
        }
    }

    [PunRPC]
    public void NetworkDrop(Vector3 dropPosition)
    {
        if (!isBeingGrabbed) return;

        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false;
        isBeingGrabbed = false;

        transform.position = dropPosition + Vector3.up * 0.1f;
        objectRigidbody.AddForce(Random.insideUnitSphere * 1f, ForceMode.Impulse);

        questPlayer.DropObject(namaBenda);
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null && isBeingGrabbed)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);

            Quaternion newRotation = Quaternion.Lerp(transform.rotation, objectGrabPointTransform.rotation, Time.deltaTime * lerpSpeed);
            objectRigidbody.MoveRotation(newRotation);
        }
    }

    public bool CanBeGrabbed()
    {
        return !isBeingGrabbed && objectRigidbody != null;
    }

    private void OnDestroy()
    {
        if (objectGrabPointTransform != null && objectGrabPointTransform.name == "TempGrabPoint")
        {
            Destroy(objectGrabPointTransform.gameObject);
        }
    }
}