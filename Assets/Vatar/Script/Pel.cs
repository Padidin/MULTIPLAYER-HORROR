using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pel : MonoBehaviour
{
    public Outline[] Outline;
    public AudioSource pickupSfx;
    public float interactDistance = 1.5f;
    public Transform inspectHolder;

    public Rigidbody rb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DropItem();
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            Pel laci = hit.collider.GetComponent<Pel>();
            if (laci != null && laci == this)
            {
                foreach (Outline outline in Outline)
                {
                    outline.eraseRenderer = false;
                }

                if (Input.GetKeyDown(KeyCode.E) && ItemInspectManager.Instance.currentItem == null)
                {
                    rb.isKinematic = true;
                    gameObject.transform.SetParent(inspectHolder);
                    transform.position = inspectHolder.position;
                    pickupSfx.Play();
                }

            }
            else
            {
                foreach (Outline outline in Outline)
                {
                    outline.eraseRenderer = true;
                }
            }
        }
        else
        {
            foreach (Outline outline in Outline)
            {
                outline.eraseRenderer = true;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Quest"))
        {
            rb.isKinematic = true;
        }
    }
}
