using Photon.Pun;
using UnityEngine;

public class CanvasIsMine : MonoBehaviourPun
{
    void Awake()
{
    if (!photonView.IsMine)
    {
        Debug.Log("Canvas dimatikan karena bukan milik saya: " + gameObject.name);
        gameObject.SetActive(false);
    }
    else
    {
        Debug.Log("Canvas aktif milik saya: " + gameObject.name);
    }
}
}
