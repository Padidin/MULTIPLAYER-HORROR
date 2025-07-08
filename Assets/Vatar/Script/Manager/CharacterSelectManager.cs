using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Photon.Pun;
using Photon.Realtime;

public class CharacterSelectManager : MonoBehaviourPunCallbacks
{
    public PlayableDirector SwitchChar1;
    public PlayableDirector SwitchChar2;
    public GameObject OwnerOnly;
    public bool char1 = true;
    public bool char2;

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            OwnerOnly.SetActive(true);
        }
        else
        {
            OwnerOnly.SetActive(false);
        }
    }

    public void SwitchCharacter1()
    {
        if (char1 == false)
        {
            photonView.RPC("ClientChar1", RpcTarget.All);
            
        }
    }

    public void SwitchCharacter2()
    {
        if (char2 == false)
        {
            photonView.RPC("ClientChar2", RpcTarget.All);
        }
    }

    [PunRPC]
    void ClientChar1()
    {
        SwitchChar1.Play();
        SwitchChar2.Stop();
        char1 = true;
        char2 = false;
    }

    [PunRPC]
    void ClientChar2()
    {
        SwitchChar2.Play();
        SwitchChar1.Stop();
        char2 = true;
        char1 = false;
    }
}
