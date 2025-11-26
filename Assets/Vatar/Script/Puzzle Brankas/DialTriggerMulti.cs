using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialTriggerMulti : MonoBehaviour, IInteractable
{
    public PuzzleBrankasMulti puzzleBrankas;
    public Outline Outline;
    public Playere Player;

    public PlayableDirector focus;
    public PlayableDirector unfocus;

    public void Highlight(bool state)
    {
        if (!puzzleBrankas.isUnlocked)
            Outline.eraseRenderer = !state;
        else
            Outline.eraseRenderer = true;
    }

    public void Interact(Playere playerMove)
    {
        if (puzzleBrankas.isUnlocked) return;

        Player = playerMove;

        if (puzzleBrankas.focused)
        {
            // keluar dari mode safe
            puzzleBrankas.focused = false;
            unfocus.Play();
            focus.Stop();
            Invoke(nameof(delayWalk), 2f);
            puzzleBrankas.currentIndex = 0;
        }
        else
        {
            // masuk ke mode safe
            //PlayerSingle.instance.canWalk = false;
            Player.canWalk = false;
            focus.Play();
            unfocus.Stop();
            Invoke(nameof(delayFocus), 2f);
        }
    }

    void delayFocus()
    {
        puzzleBrankas.focused = true;
    }

    void delayWalk()
    {
        if (Player != null)
        {
            //PlayerSingle.instance.canWalk = true;
            Player.canWalk = true;
            Player = null;
        }
    }

    public void unFocus()
    {
        puzzleBrankas.focused = false;
        unfocus.Play();
        focus.Stop();
        Invoke(nameof(delayWalk), 2f);
    }
}
