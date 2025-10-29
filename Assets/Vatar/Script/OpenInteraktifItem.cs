using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInteraktifItem : MonoBehaviour
{
    public Animator animasi;
    public bool open;
    public float delaySound;
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
            OpenInteraktifItem laci = hit.collider.GetComponent<OpenInteraktifItem>();
            if (laci != null && laci == this)
            {
                Outline.eraseRenderer = false;

                if (open)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        open = false;
                        animasi.SetBool("open", false);
                        if (bukaLaci != null && tutupLaci)
                        {
                            bukaLaci.Stop();
                            tutupLaci.PlayDelayed(delaySound);
                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        open = true;
                        animasi.SetBool("open", true);

                        if (bukaLaci != null && tutupLaci)
                        {
                            bukaLaci.Play();
                            tutupLaci.Stop();
                        }
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
