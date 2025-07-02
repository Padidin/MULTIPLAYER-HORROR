using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingLobbyManager : MonoBehaviour
{
    public Text kodeRoomTeks;

    [Header("PlayerProps")]
    public Text Player1;
    public Text Player2;

    private void Start()
    {
        kodeRoomTeks.text = PhotonNetwork.CurrentRoom.Name;
    }

    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length > 0)
        {
            Player Nickname1 = PhotonNetwork.PlayerList[0];
            Player1.text = Nickname1.NickName.ToString();
        }

        if (PhotonNetwork.PlayerList.Length > 1)
        {
            Player Nickname2 = PhotonNetwork.PlayerList[1];
            Player2.text = Nickname2.NickName.ToString();
        }
    }
}
