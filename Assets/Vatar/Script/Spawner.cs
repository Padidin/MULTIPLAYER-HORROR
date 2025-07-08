using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Spawner : MonoBehaviourPunCallbacks
{
    public GameObject Player1;
    public GameObject Player2;

    public Transform player1POS;
    public Transform player2POS;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Player1.name, player1POS.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(Player2.name, player2POS.position, Quaternion.identity);
        }
    }
}
