using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public Transform spawnPointPlayer1;
    public Transform spawnPointPlayer2;

    void Start()
    {
        string prefabName = "DefaultPlayer";

        object prefabProp;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("prefabName", out prefabProp))
        {
            prefabName = prefabProp.ToString();
        }

        Transform spawnPoint = null;

        object roleProp;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("role", out roleProp))
        {
            if (roleProp.ToString() == "creator")
            {
                spawnPoint = spawnPointPlayer1;
                Debug.Log("Player CREATOR spawn di kiri");
            }
        }
        else
        {
            spawnPoint = spawnPointPlayer2;
            Debug.Log("Player JOINER spawn di kanan");
        }

        if (spawnPoint != null)
        {
            PhotonNetwork.Instantiate(prefabName, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Spawn point tidak ditemukan!");
        }
    }
}
