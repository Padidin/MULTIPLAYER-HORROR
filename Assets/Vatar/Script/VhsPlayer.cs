using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class VhsPlayer : MonoBehaviour
{
    public float interactDistance = 1.5f;
    public Outline Outline;
    public Transform inspectHolder;
    public GameObject VHSlog1;
    public GameObject VHSlog2;

    public PlayableDirector VHStaper1;
    public PlayableDirector VHStaper2;

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            VhsPlayer laci = hit.collider.GetComponent<VhsPlayer>();
            if (laci != null && laci == this)
            {
                Outline.eraseRenderer = false;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    GameObject children = inspectHolder.GetChild(0).gameObject;

                    if (children = VHSlog1)
                    {
                        VHStaper1.Play();
                    }
                    if (children = VHSlog2)
                    {
                        VHStaper2.Play();
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
