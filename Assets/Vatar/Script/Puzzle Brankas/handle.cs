using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handle : MonoBehaviour
{
    public float interactDistance = 1.5f;
    public Outline Outline;
    public Animator animator;
    public bool terbuka;
    public bool sudahBukaBrankas;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance) && !sudahBukaBrankas)
        {
            handle laci = hit.collider.GetComponent<handle>();
            if (laci != null && laci == this)
            {
                Outline.eraseRenderer = false;

                if (Input.GetKeyDown(KeyCode.E) && terbuka)
                {
                    animator.SetTrigger("open");
                    sudahBukaBrankas = true;
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
