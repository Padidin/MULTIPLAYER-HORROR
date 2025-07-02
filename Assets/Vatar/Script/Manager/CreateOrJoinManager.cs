using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateOrJoinManager : MonoBehaviourPunCallbacks
{
    public string namaSceneWaitingLobby;
    public string namaSceneMainMenu;
    public InputField kodeJoin;
    private int KodeRoom;

    public void CreateRoomButton()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        KodeRoom = Random.Range(10000, 99999);
        PhotonNetwork.CreateRoom(KodeRoom.ToString(), roomOptions);
    }

    public void JoinRoomButton()
    {
        PhotonNetwork.JoinRoom(kodeJoin.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(namaSceneWaitingLobby);
    }

    public void BackToMainMenuButton()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene(namaSceneMainMenu);
    }
}
