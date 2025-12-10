using UnityEngine;
using Photon.Pun;
using cakeslice;

public class ObjectGrabbable : MonoBehaviourPun
{
    private Rigidbody objectRigidbody;
    private InspectMultiplayer inspectMulti;
    public Outline[] outlineObject;
    public MeshRenderer[] renderObjek;
    public Collider colliderObjek;
    public QuestPlayer questPlayer;
    public Transform holderTrans;
    public bool inGrab;
    public string namaBenda;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();

        GameObject questObjek = GameObject.Find("Quest Object");
        if (questObjek != null)
            questPlayer = questObjek.GetComponent<QuestPlayer>();

        holderTrans = GameObject.Find("Inspect Holder").transform;

        OutlineHilang();
    }

    // OUTLINE LOKAL ONLY
    public void OutlineMuncul()
    {
        foreach (Outline outline in outlineObject)
            outline.eraseRenderer = false;
    }

    public void OutlineHilang()
    {
        foreach (Outline outline in outlineObject)
            outline.eraseRenderer = true;
    }

    // ================================
    //  GRAB MULTIPLAYER
    // ================================
    public void Grab(string namaPlayer)
    {
        // Request ownership supaya object bisa dikontrol player ini
        photonView.RequestOwnership();

        // Panggil RPC ke semua client (buffered biar join belakangan tetap kayak real-time)
        photonView.RPC("RPC_Grab", RpcTarget.AllBuffered, namaPlayer);
    }

    [PunRPC]
    void RPC_Grab(string namaPlayer)
    {
        objectRigidbody.useGravity = false;
        objectRigidbody.isKinematic = true;
        inGrab = true;

        OutlineHilang();
        questPlayer.GrabObject(namaBenda);

        transform.SetParent(holderTrans);
        transform.position = holderTrans.position;
        colliderObjek.isTrigger = true;

        if (namaPlayer == "Argha")
        {
            GameObject inspect = GameObject.Find(namaPlayer);
            if (inspect != null)
            {
                inspectMulti = inspect.GetComponent<InspectMultiplayer>();
            }
        }
    }

    // ================================
    //  DROP MULTIPLAYER
    // ================================
    public void Drop(string namaPlayer)
    {
        photonView.RPC("RPC_Drop", RpcTarget.AllBuffered, namaPlayer);
    }

    [PunRPC]
    void RPC_Drop(string namaPlayer)
    {
        if (inspectMulti != null)
            inspectMulti.DropItem();

        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false;
        inGrab = false;

        questPlayer.DropObject(namaBenda);
        colliderObjek.isTrigger = false;

        transform.SetParent(null);
    }
}