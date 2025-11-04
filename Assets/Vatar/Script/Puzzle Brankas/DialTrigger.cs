using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialTrigger : MonoBehaviour
{
    public PuzzleBrankas puzzleBrankas;
    public Outline Outline;
    public float interactDistance = 1.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            DialTrigger laci = hit.collider.GetComponent<DialTrigger>();
            if (laci != null && laci == this)
            {
                Outline.eraseRenderer = false;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (puzzleBrankas.focused)
                    {
                        puzzleBrankas.focused = false;
                    }
                    else
                    {
                        puzzleBrankas.focused = true;
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
}
