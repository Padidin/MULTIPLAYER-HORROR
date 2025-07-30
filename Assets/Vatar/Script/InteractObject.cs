using System.Collections;
using UnityEngine;
using Photon.Pun;

public class InteractObject : MonoBehaviourPunCallbacks
{
    public bool triggerBathup = false;
    public bool triggerRak = false;

    private void Update()
    {
        if (!photonView.IsMine) return; // hanya local player yang bisa input

        if (triggerBathup && Input.GetKeyDown(KeyCode.E))
        {
            CallRPCToOther("InteractBathup");
        }

        if (triggerRak && Input.GetKeyDown(KeyCode.E))
        {
            CallRPCToOther("InteractRak");
        }
    }

    void CallRPCToOther(string rpcName)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in players)
        {
            PhotonView view = p.GetComponent<PhotonView>();

            // Cari player lain (bukan kita sendiri)
            if (view != null && !view.IsMine)
            {
                view.RPC(rpcName, view.Owner, null); // panggil hanya ke player tersebut
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TombolBathup")) triggerBathup = true;
        if (other.CompareTag("TombolRak")) triggerRak = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TombolBathup")) triggerBathup = false;
        if (other.CompareTag("TombolRak")) triggerRak = false;
    }
}
