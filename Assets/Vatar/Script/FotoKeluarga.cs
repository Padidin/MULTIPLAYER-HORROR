using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FotoKeluarga : MonoBehaviour
{
    public Outline Outline;
    public float interactDistance = 1.5f;
    public AudioSource PickupSfx;
    private Transform holder;

    private Rigidbody rb;
    // Start is called before the first frame update
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
            FotoKeluarga laci = hit.collider.GetComponent<FotoKeluarga>();
            if (laci != null && laci == this)
            {
                Outline.eraseRenderer = false;

                if (Input.GetKeyDown(KeyCode.E) && ItemInspectManager.Instance.currentItem == null)
                {
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
        gameObject.transform.position = holder.position;
        PickupSfx.Play();
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
