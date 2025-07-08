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

    public void SwtichCharacter1()
    {
        if (char1 == false)
        {
            SwitchChar1.Play();
            SwitchChar2.Stop();
            char1 = true;
            char2 = false;
        }
    }

    public void SwtichCharacter2()
    {
        if (char2 == false)
        {
            SwitchChar2.Play();
            SwitchChar1.Stop();
            char2 = true;
            char1 = false;
        }
    }
}
