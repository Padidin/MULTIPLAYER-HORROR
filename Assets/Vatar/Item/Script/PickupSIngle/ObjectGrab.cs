using cakeslice;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ObjectGrab : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;

    public Outline[] outlineObject;
    public QuestPlayer questPlayer;
    public bool inGrab;
    public string namaBenda;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();

        GameObject questObjek = GameObject.Find("Quest Object");
        questPlayer = questObjek.GetComponent<QuestPlayer>();

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

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;

        objectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        inGrab = true;
        OutlineHilang();
        questPlayer.GrabObject(namaBenda);
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;

        objectRigidbody.constraints = RigidbodyConstraints.None;
        inGrab = false;
        questPlayer.DropObject(namaBenda);
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);

            Quaternion newRotation = Quaternion.Lerp(transform.rotation, objectGrabPointTransform.rotation, Time.deltaTime * lerpSpeed);
            objectRigidbody.MoveRotation(newRotation);
        }
    }
}