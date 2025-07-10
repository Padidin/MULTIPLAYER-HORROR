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
        string kodeRoom = Random.Range(10000, 99999).ToString();
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(kodeRoom, options);

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props["prefabName"] = "Player1Prefab";
        props["role"] = "creator";
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void JoinRoomButton()
    {
        PhotonNetwork.JoinRoom(kodeJoin.text);

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props["prefabName"] = "Player2Prefab";
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
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
