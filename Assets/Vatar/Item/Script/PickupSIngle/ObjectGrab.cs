using cakeslice;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ObjectGrab : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;

    [SerializeField] private InspectMultiplayer inspectMulti;
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
        questPlayer = questObjek.GetComponent<QuestPlayer>();
        holderTrans = GameObject.Find("Inspect Holder").transform;

        OutlineHilang();
    }

    public void OutlineMuncul()
    {
        foreach (Outline outline in outlineObject)
        {
            outline.eraseRenderer = false;
        }
    }
    public void OutlineHilang()
    {
        foreach (Outline outline in outlineObject)
        {
            outline.eraseRenderer = true;
        }
    }

    public void Grab(string namaPlayer)
    {
        objectRigidbody.useGravity = false;

        objectRigidbody.isKinematic = true;
        inGrab = true;
        OutlineHilang();
        questPlayer.GrabObject(namaBenda);

        //foreach (MeshRenderer render in renderObjek)
        //{
        //    render.enabled = false;
        //}

        gameObject.transform.SetParent(holderTrans);
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

    public void Drop(string namaPlayer)
    {
        if (inspectMulti != null)
            inspectMulti.DropItem();

        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false;
        inGrab = false;
        questPlayer.DropObject(namaBenda);

        colliderObjek.isTrigger = false;

    }


    /*private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);

            Quaternion newRotation = Quaternion.Lerp(transform.rotation, objectGrabPointTransform.rotation, Time.deltaTime * lerpSpeed);
            objectRigidbody.MoveRotation(newRotation);
        }
    }*/
}