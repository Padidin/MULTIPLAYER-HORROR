using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingLobbyManager : MonoBehaviour
{
    public Text kodeRoomTeks;

    private void Start()
    {
        kodeRoomTeks.text = PhotonNetwork.CurrentRoom.Name;
    }
}
