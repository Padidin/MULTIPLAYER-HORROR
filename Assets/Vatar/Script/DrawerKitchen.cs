using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerKitchen : MonoBehaviour
{
    public Animator animasi;
    public bool open;
    public AudioSource bukaLaci;
    public AudioSource tutupLaci;

    public Outline Outline;

    public float interactDistance = 1.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            DrawerKitchen laci = hit.collider.GetComponent<DrawerKitchen>();
            if (laci != null && laci == this)
            {
                Outline.eraseRenderer = false;

                if (open)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        open = false;
                        animasi.SetBool("open", false);
                        bukaLaci.Stop();
                        tutupLaci.PlayDelayed(0.3f);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        open = true;
                        animasi.SetBool("open", true);
                        bukaLaci.Play();
                        tutupLaci.Stop();
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

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Argha") || other.CompareTag("Irul"))
        {
            terjangkau = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Argha") || other.CompareTag("Irul"))
        {
            terjangkau = false;
        }
    }*/
}
