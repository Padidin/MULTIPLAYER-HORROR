using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialTrigger : MonoBehaviour
{
    public PuzzleBrankas puzzleBrankas;
    public Outline Outline;
    public float interactDistance = 1.5f;
    public handle Handle;

    public PlayableDirector focus;
    public PlayableDirector unfocus;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance) && !Handle.sudahBukaBrankas)
        {
            DialTrigger laci = hit.collider.GetComponent<DialTrigger>();
            if (laci != null && laci == this)
            {
                if (!puzzleBrankas.focused)
                {
                    Outline.eraseRenderer = false;
                }
                else
                {
                    Outline.eraseRenderer = true;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (puzzleBrankas.focused)
                    {
                        puzzleBrankas.focused = false;
                        unfocus.Play();
                        focus.Stop();
                        Invoke(nameof(delayCanWalk), 2f);
                        puzzleBrankas.currentIndex = 0;
                    }
                    else
                    {
                        Invoke(nameof(delayFocus), 2f);
                        PlayerSingle.instance.canWalk = false;
                        focus.Play();
                        unfocus.Stop();
                    }
                }

            }
            else
            {
                Outline.eraseRenderer = true;
            }
        }
        else
        {
            Outline.eraseRenderer = true;
        }
    }

    void delayFocus()
    {
        puzzleBrankas.focused = true;
    }

    void delayCanWalk()
    {
        PlayerSingle.instance.canWalk = true;
    }

    public void unFocus()
    {
        puzzleBrankas.focused = false;
        unfocus.Play();
        focus.Stop();
        Invoke(nameof(delayCanWalk), 2f);
    }
}
