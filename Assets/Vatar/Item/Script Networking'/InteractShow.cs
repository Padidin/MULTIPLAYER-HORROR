using TMPro;
using UnityEngine;
using Photon.Pun;

public class InteractShow : MonoBehaviourPun
{
    public static InteractShow instance;

    public TextMeshProUGUI interactShow;

    private void Awake()
    {
        instance = this;

        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/
    }

    [PunRPC]
    public void Show()
    {
        interactShow.text = "Press E to interact";
    }

    [PunRPC]
    public void Hide()
    {
        interactShow.text = "";
    }

    // Method untuk local call saja
    public void LocalShow()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("Show", RpcTarget.All);
        }
    }

    public void LocalHide()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("Hide", RpcTarget.All);
        }
    }
}