using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;

public class Nickname : MonoBehaviourPun
{
    public Text nicknameText;

    void Start()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SetNickname", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
    }

    [PunRPC]
    void SetNickname(string nick)
    {
        nicknameText.text = nick;
    }
}
