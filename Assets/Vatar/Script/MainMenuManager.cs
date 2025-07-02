using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public string accountName;
    public InputField inputNameAccount;
    public Text usernameTeks;

    public void ConfirmAccountName()
    {
        if (inputNameAccount.text != null)
        {
            PlayerPrefs.SetString("username", inputNameAccount.text);
        }
    }

    private void Update()
    {
        accountName = PlayerPrefs.GetString("username");
        usernameTeks.text = "User : " + accountName;
    }

    public void MultiplayerButton()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            SceneManager.LoadScene("LoadingScene");
        }   
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
