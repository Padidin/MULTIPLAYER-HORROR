using Photon.Pun;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public class SpawnData
    {
        public string prefabName;
        public Transform spawnPoint;
    }

    public SpawnData[] itemsToSpawn;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var item in itemsToSpawn)
            {
                PhotonNetwork.Instantiate(item.prefabName, item.spawnPoint.position, item.spawnPoint.rotation);
            }
        }
    }
}
