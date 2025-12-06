using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WarningFull : MonoBehaviour
{
    public static WarningFull instance;
    public PlayableDirector showText;

    private void Awake()
    {
        instance = this;
    }

    public void StartShowing()
    {
        showText.Play();
    }
}
