using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuestMultiplayer : MonoBehaviourPunCallbacks
{
    public GameObject letakPisau;
    public GameObject letakPel;
    public Outline[] outlinePisau;
    public Outline[] outlinePel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pisau"))
        {
            //letakPisau.SetActive(true);
            photonView.RPC("PisauAktif", RpcTarget.All);
            PhotonNetwork.Destroy(other.gameObject);
            
        }
        if (other.CompareTag("Pel"))
        {
            photonView.RPC("PelAktif", RpcTarget.All);
            PhotonNetwork.Destroy(other.gameObject);
        }
    }

    [PunRPC]
    void PisauAktif()
    {
        letakPisau.SetActive(true);
    }

    [PunRPC]
    void PelAktif()
    {
        letakPel.SetActive(true);
    }
    public void GrabObject(string bendaApa)
    {
        if (bendaApa == "Pisau")
        {
            foreach (var objek in outlinePisau)
            {
                objek.eraseRenderer = false;
            }
        }
        else if (bendaApa == "Pel")
        {
            foreach (var objek in outlinePel)
            {
                objek.eraseRenderer = false;
            }
        }
    }

    public void DropObject(string bendaApa)
    {
        if (bendaApa == "Pisau")
        {
            foreach (var objek in outlinePisau)
            {
                objek.eraseRenderer = true;
            }
        }
        else if (bendaApa == "Pel")
        {
            foreach (var objek in outlinePel)
            {
                objek.eraseRenderer = true;
            }
        }
    }
}
