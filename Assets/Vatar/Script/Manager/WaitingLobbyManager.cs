using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingLobbyManager : MonoBehaviourPunCallbacks
{
    public Text kodeRoomTeks;
    public Text TeksButton;
    public string namaSceneSelanjutnya;
    public string namaSceneSebelumnya;

    [Header("PlayerProps")]
    public Text Player1;
    public Text Player2;
    public Text statusPlayer2;

    public bool ClientReady;

    private void Start()
    {
        kodeRoomTeks.text = PhotonNetwork.CurrentRoom.Name;
        PhotonNetwork.AutomaticallySyncScene = true;
        RoomProperties();
    }

    private void Update()
    {
        CekPlayer();
        CekStatusPlayer();
    }

    void RoomProperties()
    {
        Hashtable roomProps = new Hashtable();
        roomProps["ClientReady"] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
    }

    void CekPlayer()
    {
        if (PhotonNetwork.PlayerList.Length == 1)
        {
            Player Nickname1 = PhotonNetwork.PlayerList[0];
            Player1.text = Nickname1.NickName.ToString();

            Player2.text = "?????";
            statusPlayer2.text = "-";
        }

        if (PhotonNetwork.PlayerList.Length > 1)
        {
            Player Nickname1 = PhotonNetwork.PlayerList[0];
            Player1.text = Nickname1.NickName.ToString();

            Player Nickname2 = PhotonNetwork.PlayerList[1];
            Player2.text = Nickname2.NickName.ToString();
        }
    }

    void CekStatusPlayer()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            if (!ClientReady)
            {
                statusPlayer2.text = "Unready";
            }
            else
            {
                statusPlayer2.text = "Ready";
            }
        }
        else
        {
            Player2.text = "?????";
            statusPlayer2.text = "-";
        }

        if (PhotonNetwork.IsMasterClient)
        {
            TeksButton.text = "START";
        }
        else
        {
            TeksButton.text = "READY";
        }

        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("ClientReady", out object value))
        {
            ClientReady = (bool)value;
        }
    }

    public void StartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (ClientReady)
            {
                PhotonNetwork.LoadLevel(namaSceneSelanjutnya);
            }
        }
        
        if (!PhotonNetwork.IsMasterClient)
        {
            if (ClientReady)
            {
                Hashtable roomProps = new Hashtable();
                roomProps["ClientReady"] = false;
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
            }
            else
            {
                Hashtable roomProps = new Hashtable();
                roomProps["ClientReady"] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
            }
        }
    }

    public void LeaveButton()
    {
        if (PhotonNetwork.InRoom)
        {
            Hashtable roomProps = new Hashtable();
            roomProps["ClientReady"] = false;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);

            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(namaSceneSebelumnya);

    }

}
