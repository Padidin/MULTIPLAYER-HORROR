using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class SceneLoadHandler : MonoBehaviourPunCallbacks
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Daftarkan event saat scene selesai dimuat
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unregister event-nya
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Scene loaded, memulai spawn player...");
            FindObjectOfType<Spawner>().StartSpawn(); // Panggil fungsi spawn player
        }
    }
}
