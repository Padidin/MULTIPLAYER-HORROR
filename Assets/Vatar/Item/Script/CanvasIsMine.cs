using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class CanvasIsMine : MonoBehaviourPunCallbacks
{
    public Canvas myCanvas;

    void Start()
    {
        if (!photonView.IsMine && myCanvas != null)
        {
            myCanvas.gameObject.SetActive(false);
        }
    }

}
