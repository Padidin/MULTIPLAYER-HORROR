using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ObjectGrab : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;

        objectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;

        objectRigidbody.constraints = RigidbodyConstraints.None;
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