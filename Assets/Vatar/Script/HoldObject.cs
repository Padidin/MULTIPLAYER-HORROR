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
    public bool triggerRak;

    public Transform bathupObject;
    public Transform bathupTarget;
    public Transform rakObject;
    public Transform rakTarget;
    public float moveSpeed = 1f;
    private void Start()
    {
        GameObject bathupObj = GameObject.FindGameObjectWithTag("Bathup");
        GameObject targetBathup = GameObject.FindGameObjectWithTag("BathupTarget");

        GameObject rakObj = GameObject.FindGameObjectWithTag("Rak");
        GameObject targetRak = GameObject.FindGameObjectWithTag("RakTarget");

        bathupObject = bathupObj.transform;
        bathupTarget = targetBathup.transform;
        
        rakObject = rakObj.transform;
        rakTarget = targetRak.transform;
    }

    private void Update()
    {
        if (canInteractBathup && triggerBathup && Input.GetKey(KeyCode.E))
        {
            bathupObject.position = Vector3.MoveTowards(bathupObject.position, bathupTarget.position, moveSpeed * Time.deltaTime);
        }

        if (canInteractRak && triggerRak && Input.GetKey(KeyCode.E))
        {
            rakObject.position = Vector3.MoveTowards(rakObject.position, rakTarget.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bathup"))
        {
            triggerBathup = true;
        }
        if (other.CompareTag("Rak"))
        {
            triggerRak = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bathup"))
        {
            triggerBathup = false;
        }
        if (other.CompareTag("Rak"))
        {
            triggerRak = false;
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
