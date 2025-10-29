using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedKeyPuzzle : MonoBehaviour
{
    public DoorWood ScriptPintu;
    public AudioSource PickupSfx;

    public Outline Outline;

    public float interactDistance = 1.5f;

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            RedKeyPuzzle laci = hit.collider.GetComponent<RedKeyPuzzle>();
            if (laci != null && laci == this)
            {
                Outline.eraseRenderer = false;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    ScriptPintu.gotKey = true;
                    AudioSource.PlayClipAtPoint(PickupSfx.clip, transform.position);
                    Destroy(gameObject);
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

