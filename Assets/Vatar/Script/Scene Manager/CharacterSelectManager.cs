using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviourPunCallbacks
{
    public PlayableDirector SwitchChar1;
    public PlayableDirector SwitchChar2;
    public GameObject OwnerOnly;
    public Text countdownText;
    public bool char1 = true;
    public bool char2;
    public string NextScene;

    private bool hasSelectedCharacter = false;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            OwnerOnly.SetActive(true);
        }
        else
        {
            OwnerOnly.SetActive(false);
        }
    }

    public void SelectCharacterButton()
    {
        if (hasSelectedCharacter) return;
        hasSelectedCharacter = true;

        if (char1)
        {
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
            playerProps["ChosenCharacter"] = "Karakter1";
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        }
        else if (char2)
        {
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
            playerProps["ChosenCharacter"] = "Karakter2";
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        }

        StartCoroutine(LoadMultiPlayer());
    }

    IEnumerator LoadMultiPlayer()
    {
        int countdown = 5;

        while (countdown > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = "GAME STARTING IN " + countdown + "...";
            }

            yield return new WaitForSeconds(1f);
            countdown--;
        }

        if (countdownText != null)
        {
            countdownText.text = "Loading...";
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(NextScene);
        }
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer.IsMasterClient && changedProps.ContainsKey("ChosenCharacter"))
        {
            string masterChoice = changedProps["ChosenCharacter"].ToString();
            string myChoice = masterChoice == "Karakter1" ? "Karakter2" : "Karakter1";

            ExitGames.Client.Photon.Hashtable myProps = new ExitGames.Client.Photon.Hashtable();
            myProps["ChosenCharacter"] = myChoice;
            PhotonNetwork.LocalPlayer.SetCustomProperties(myProps);
        }
    }

    public void SwitchCharacter1()
    {
        if (char1 == false)
        {
            photonView.RPC("ClientChar1", RpcTarget.All);
            
        }
    }

    public void SwitchCharacter2()
    {
        if (char2 == false)
        {
            photonView.RPC("ClientChar2", RpcTarget.All);
        }
    }

    [PunRPC]
    void ClientChar1()
    {
        SwitchChar1.Play();
        SwitchChar2.Stop();
        char1 = true;
        char2 = false;
    }

    [PunRPC]
    void ClientChar2()
    {
        SwitchChar2.Play();
        SwitchChar1.Stop();
        char2 = true;
        char1 = false;
    }
}
