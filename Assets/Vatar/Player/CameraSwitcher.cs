using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera cameraPlayer1;
    public Camera cameraPlayer2;

    private bool isPlayer1View = true;

    void Start()
    {
        SetCamera(true); // mulai dari player 1
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isPlayer1View = !isPlayer1View;
            SetCamera(isPlayer1View);
        }
    }

    void SetCamera(bool viewPlayer1)
    {
        cameraPlayer1.gameObject.SetActive(viewPlayer1);
        cameraPlayer2.gameObject.SetActive(!viewPlayer1);
    }

    public bool IsPlayer1ViewActive()
    {
        return isPlayer1View;
    }
}
