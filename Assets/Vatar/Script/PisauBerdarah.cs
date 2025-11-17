using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PisauBerdarah : MonoBehaviour
{
    public Outline[] outline;
    public AudioSource pickupSfx;
    public float interactDistance = 1.5f;
    private Transform holder;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        holder = GameObject.Find("Inspect Holder").transform;
    }

    // Update is called once per frame
    void Update()
    {
        DropItem();
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            PisauBerdarah laci = hit.collider.GetComponent<PisauBerdarah>();
            if (laci != null && laci == this)
            {
                foreach (Outline border in outline)
                {
                    border.eraseRenderer = false;
                }

                if (Input.GetKeyDown(KeyCode.E) && ItemInspectManager.Instance.currentItem == null)
                {
                    PickupItem();
                }

            }
            else
            {
                foreach (Outline border in outline)
                {
                    border.eraseRenderer = true;
                }
            }
        }
        else
        {
            foreach (Outline border in outline)
            {
                border.eraseRenderer = true;
            }
        }
    }

    void PickupItem()
    {
        rb.isKinematic = true;
        gameObject.transform.SetParent(holder);
        transform.position = holder.position;
        pickupSfx.Play();
    }

    void DropItem()
    {
        if (!transform.IsChildOf(holder)) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            ItemInspectManager.Instance.DropItem();
            rb.isKinematic = false;
        }
    }
}
