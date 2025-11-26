using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handleMulti : MonoBehaviour, IInteractable
{
    public Outline Outline;
    public Animator animator;
    public bool terbuka;
    public bool sudahBukaBrankas;
    public AudioSource openSfx;

    public void Highlight(bool state)
    {
        Outline.eraseRenderer = !state;
    }

    public void Interact(Playere namaPlayer)
    {
        if (sudahBukaBrankas) return;

        if (terbuka)
        {
            animator.SetTrigger("open");
            openSfx?.Play();
            sudahBukaBrankas = true;
        }
    }
}
