using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutscene : MonoBehaviour
{
    public PlayableDirector cutscene;
    private bool sudahKePlay = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Argha"))
        {
            if (sudahKePlay == false)
            {
                sudahKePlay = true;
               cutscene.Play();
            }
        }
    }
}
