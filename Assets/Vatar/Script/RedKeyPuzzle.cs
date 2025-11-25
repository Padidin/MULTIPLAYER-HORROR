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

    private Transform holder;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        holder = GameObject.Find("Inspect Holder").transform;
    }

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

                if (Input.GetKeyDown(KeyCode.E) && ItemInspectManager.Instance.currentItem == null)
                {
                    ScriptPintu.gotKey = true;
                    PickupItem();
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

        DropItem();
    }

    void PickupItem()
    {
        rb.isKinematic = true;
        gameObject.transform.SetParent(holder);
        transform.position = holder.position;
        PickupSfx.Play();
    }

    void DropItem()
    {
        if (!transform.IsChildOf(holder)) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            ItemInspectManager.Instance.DropItem();
            rb.isKinematic = false;
            ScriptPintu.gotKey = false;
        }
    }
}