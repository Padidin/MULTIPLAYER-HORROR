using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InteractObject : MonoBehaviourPunCallbacks
{
    public bool triggerBathup = false;
    public bool triggerRak = false;

    private void Update()
    {
        if (triggerBathup && Input.GetKeyDown(KeyCode.E))
        {
            photonView.RPC("InteractBathup", RpcTarget.Others);
        }

        if (triggerRak && Input.GetKeyDown(KeyCode.E))
        {
            photonView.RPC("InteractRak", RpcTarget.Others);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TombolBathup"))
        {
            triggerBathup = true;
        }
        if (other.CompareTag("TombolRak"))
        {
            triggerRak = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TombolBathup"))
        {
            triggerBathup = false;
        }
        if (other.CompareTag("Tombol Rak"))
        {
            triggerRak = false;
        }
    }

}
