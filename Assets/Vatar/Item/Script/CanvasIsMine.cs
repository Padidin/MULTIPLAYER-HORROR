using Photon.Pun;

public class CanvasIsMine : MonoBehaviourPun
{
    void Start()
    {
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }
}
