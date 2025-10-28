using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFlaslight : MonoBehaviour
{
    public Outline Outline;
    public Flashlight Flashlight;

    public float interactDistance = 1.5f;

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            ItemFlaslight laci = hit.collider.GetComponent<ItemFlaslight>();
            if (laci != null && laci == this)
            {
                Outline.eraseRenderer = false;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Flashlight.haveFlashlight = true;
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
