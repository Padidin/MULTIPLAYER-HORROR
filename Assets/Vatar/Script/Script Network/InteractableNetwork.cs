using UnityEngine;
using Photon.Pun;

public class InteractableNetwork : MonoBehaviourPun, IInteractable
{
    public bool isInteracted = false;

    public void Highlight(bool state)
    {
        // local only (efek visual)
        // jangan diRPC!
    }

    public void Interact(Playere playerMove)
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }

        if (!isInteracted)
        {
            photonView.RPC("RPC_SetInteracted", RpcTarget.AllBuffered, true);
            // Lakuin efek interaksi (buka pintu, ambil item, dsb)
        }
        else
        {
            Debug.Log("Udah di-interact sama player lain bro.");
        }
    }

    [PunRPC]
    void RPC_SetInteracted(bool state)
    {
        isInteracted = state;
        // efek visual/anim di semua player
    }
}
