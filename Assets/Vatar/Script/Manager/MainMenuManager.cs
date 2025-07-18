using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    public string accountName;
    public InputField inputNameAccount;
    public Text usernameTeks;

    [Header("Nama Scene")]
    public string namaSceneMultiPlayer;
    public string namaSceneSinglePlayer;

    public void ConfirmAccountName()
    {
        if (inputNameAccount.text != null)
        {
            PlayerPrefs.SetString("username", inputNameAccount.text);
            inputNameAccount.text = null;
        }
    }

    private void Update()
    {
        accountName = PlayerPrefs.GetString("username");
        PhotonNetwork.NickName = accountName;

        PhotonNetwork.NickName = accountName;

        if (PlayerPrefs.HasKey("username"))
        {
            usernameTeks.text = "User : " + accountName;
        }
        if (!PlayerPrefs.HasKey("username"))
        {
            usernameTeks.text = "User : -";
        }
    }

    public void SinglePlayerButton()
    {
        SceneManager.LoadScene(namaSceneSinglePlayer);
    }

    public void MultiplayerButton()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            SceneManager.LoadScene(namaSceneMultiPlayer);
        }   
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
