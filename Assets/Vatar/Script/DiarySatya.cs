using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiarySatya : MonoBehaviour
{
    public Outline[] outline;
    public AudioSource pickupSfx;
    public float interactDistance = 1.5f;
    public Transform inspectHolder;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        DropItem();
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            DiarySatya laci = hit.collider.GetComponent<DiarySatya>();
            if (laci != null && laci == this)
            {
                foreach (Outline line in outline)
                {
                    line.eraseRenderer = false;
                }

                if (Input.GetKeyDown(KeyCode.E) && ItemInspectManager.Instance.currentItem == null)
                {
                    rb.isKinematic = true;
                    gameObject.transform.SetParent(inspectHolder);
                    transform.position = inspectHolder.position;
                    pickupSfx.Play();
                }
                else if (Input.GetKeyDown(KeyCode.E) && ItemInspectManager.Instance.currentItem != null)
                {
                    WarningFull.instance.StartShowing();
                }

            }
            else
            {
                foreach (Outline line in outline)
                {
                    line.eraseRenderer = true;
                }
            }
        }
        else
        {
            foreach (Outline line in outline)
            {
                line.eraseRenderer = true;
            }
        }
    }

    void DropItem()
    {
        Transform holder = GameObject.Find("Inspect Holder").transform;

        if (!transform.IsChildOf(holder)) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            ItemInspectManager.Instance.DropItem();
            rb.isKinematic = false;
        }
    }
}
