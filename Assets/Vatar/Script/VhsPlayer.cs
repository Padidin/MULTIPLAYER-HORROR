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
    public GameObject VHSLog1;
    public GameObject VHSLog2;

    public PlayableDirector VHSTaper1;
    public PlayableDirector VHSTaper2;

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (inspectHolder.childCount <= 0) return;
        GameObject children = inspectHolder.GetChild(0).gameObject;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            VhsPlayer laci = hit.collider.GetComponent<VhsPlayer>();
            if (laci != null && laci == this)
            {
                Outline.eraseRenderer = false;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (children == VHSLog1)
                    {
                        VHSTaper1.Play();
                    }
                    if (children == VHSLog2)
                    {
                        VHSTaper2.Play();
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
