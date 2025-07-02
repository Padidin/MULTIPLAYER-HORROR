using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateAndJoin : MonoBehaviour
{
    [SerializeField] private int KodeRoom;
    public Text kodeText;

    public void JoinRoom()
    {

    }

    public void CreateRoom()
    {
        KodeRoom = Random.Range(10000, 99999);
        kodeText.text = KodeRoom.ToString();
    }
}
