using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HoldObject : MonoBehaviourPunCallbacks
{
    public bool canInteractBathup = false;
    public bool canInteractRak = false;

    public bool triggerBathup;

    public Transform bathupObject;
    public Transform bathupTarget;
    public float moveSpeed = 1f;
    private void Start()
    {
        GameObject bathupObj = GameObject.FindGameObjectWithTag("Bathup");
        GameObject targetBathup = GameObject.FindGameObjectWithTag("BathupTarget");

        bathupObject = bathupObj.transform;
        bathupTarget = targetBathup.transform;
    }

    private void Update()
    {
        if (canInteractBathup && triggerBathup && Input.GetKey(KeyCode.E))
        {
            bathupObject.position = Vector3.MoveTowards(bathupObject.position, bathupTarget.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bathup"))
        {
            triggerBathup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bathup"))
        {
            triggerBathup = false;
        }
    }

    [PunRPC]
    public void InteractBathup()
    {
        StartCoroutine(BathupActive());
    }
    
    [PunRPC]
    public void InteractRak()
    {
        StartCoroutine(RakActive());
    }

    IEnumerator BathupActive()
    {
        canInteractBathup = true;
        yield return new WaitForSeconds(10f);
        canInteractBathup = false;
    }
    IEnumerator RakActive()
    {
        canInteractRak = true;
        yield return new WaitForSeconds(10f);
        canInteractRak = false;
    }
}
