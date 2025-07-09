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
        StartCoroutine(SpawnCharacter());
    }

    IEnumerator SpawnCharacter()
    {
        yield return new WaitForSeconds(1f);

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("ChosenCharacter"))
        {
            string chosenChar = PhotonNetwork.LocalPlayer.CustomProperties["ChosenCharacter"].ToString();

            GameObject prefab = null;
            Transform spawnPos = null;

            if (chosenChar == "Karakter1")
            {
                prefab = Player1;
                spawnPos = player1POS;
            }
            else if (chosenChar == "Karakter2")
            {
                prefab = Player2;
                spawnPos = player2POS;
            }

            if (prefab != null && spawnPos != null)
            {
                PhotonNetwork.Instantiate(prefab.name, spawnPos.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Prefab atau posisi spawn tidak ditemukan.");
            }
        }
        else
        {
            Debug.LogError("ChosenCharacter belum diset di CustomProperties.");
        }
    }
}
