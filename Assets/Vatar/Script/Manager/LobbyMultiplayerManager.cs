using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMultiplayerManager : MonoBehaviourPunCallbacks
{
    public string namaScene;
    public InputField kodeJoin;
    [SerializeField] private int KodeRoom;

    public void CreateRoomButton()
    {
        KodeRoom = Random.Range(10000, 99999);
        PhotonNetwork.CreateRoom(KodeRoom.ToString());
        PhotonNetwork.LoadLevel(namaScene);
    }

    public void JoinRoomButton()
    {
        PhotonNetwork.JoinRoom(kodeJoin.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(namaScene);
    }
}
