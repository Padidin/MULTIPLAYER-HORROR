using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public bool haveFlashlight;
    public GameObject cahaya;

    private void Update()
    {
        if (!haveFlashlight) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (cahaya.activeInHierarchy)
            {
                cahaya.SetActive(false);
            }
            else
            {
                cahaya.SetActive(true);
            }
        }
    }
}
