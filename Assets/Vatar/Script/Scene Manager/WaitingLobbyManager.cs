using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaitingLobbyManager : MonoBehaviourPunCallbacks
{
    public Text kodeRoomTeks;
    public Text TeksButton;
    public string namaSceneSelanjutnya;
    public string namaSceneSebelumnya;

    public GameObject LoadingLayer;
    public Color Red;
    public Color Green;

    [Header("PlayerProps")]
    public Text Player1;
    public Text Player2;
    public Text statusPlayer2;

    public bool ClientReady;

    private AsyncOperation asyncLoad;

    private void Start()
    {
        kodeRoomTeks.text = PhotonNetwork.CurrentRoom.Name;
        RoomProperties();
    }

    private void Update()
    {
        CekPlayer();
        CekStatusPlayer();
        LoadingNextScene();
    }

    void RoomProperties()
    {
        Hashtable WaitiLobbyScene = new Hashtable();
        WaitiLobbyScene["ClientReady"] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(WaitiLobbyScene);
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
                statusPlayer2.text = "UnReady";
                statusPlayer2.color = Red;
            }
            else
            {
                statusPlayer2.text = "Ready";
                statusPlayer2.color = Green;
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
                photonView.RPC("StartLoadNextScene", RpcTarget.All);
            }
        }
        
        if (!PhotonNetwork.IsMasterClient)
        {
            if (ClientReady)
            {
                Hashtable WaitiLobbyScene = new Hashtable();
                WaitiLobbyScene["ClientReady"] = false;
                PhotonNetwork.CurrentRoom.SetCustomProperties(WaitiLobbyScene);
            }
            else
            {
                Hashtable WaitiLobbyScene = new Hashtable();
                WaitiLobbyScene["ClientReady"] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(WaitiLobbyScene);
            }
        }
    }

    [PunRPC]
    void StartLoadNextScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync(namaSceneSelanjutnya);
        asyncLoad.allowSceneActivation = false;
        LoadingLayer.SetActive(true);
    }

    public void LeaveButton()
    {
        if (PhotonNetwork.InRoom)
        {
            Hashtable WaitiLobbyScene = new Hashtable();
            WaitiLobbyScene["ClientReady"] = false;
            PhotonNetwork.CurrentRoom.SetCustomProperties(WaitiLobbyScene);

            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(namaSceneSebelumnya);

    }

    void LoadingNextScene()
    {
        if (asyncLoad != null && asyncLoad.progress >= 0.9f && PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("LoadDone", out object value) == true)
            {
                photonView.RPC("PindahScene", RpcTarget.All);
            }
        }

        if (asyncLoad != null && asyncLoad.progress >= 0.9f && !PhotonNetwork.IsMasterClient)
        {
            Hashtable WaitiLobbyScene = new Hashtable();
            WaitiLobbyScene["LoadDone"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(WaitiLobbyScene);
        }
    }

    [PunRPC]
    void PindahScene()
    {
        asyncLoad.allowSceneActivation = true;
    }


}
