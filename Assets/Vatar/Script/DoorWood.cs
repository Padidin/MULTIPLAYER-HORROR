using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWood : MonoBehaviour
{
    public Animator animasi;
    public bool open;
    public bool terjangkau;
    public bool haveKey;
    public AudioSource bukaPintu;
    public AudioSource tutupPintu;

    public Outline[] Outline;

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
            DoorWood pintu = hit.collider.GetComponent<DoorWood>();
            if (pintu != null && pintu == this)
            {
                foreach (Outline garisTepi in Outline)
                {
                    garisTepi.eraseRenderer = false;
                }

                if (haveKey)
                {
                    if (open)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            open = false;
                            animasi.SetBool("open", false);
                            bukaPintu.Stop();
                            tutupPintu.PlayDelayed(1.3f);
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            open = true;
                            animasi.SetBool("open", true);
                            bukaPintu.Play();
                            tutupPintu.Stop();
                        }
                    }
                }
                else
                {
                    // Mainkan Suara terkunci
                }
            }
            else
            {
                foreach (Outline garisTepi in Outline)
                {
                    garisTepi.eraseRenderer = true;
                }
            }
        }
        else
        {
            foreach (Outline garisTepi in Outline)
            {
                garisTepi.eraseRenderer = true;
            }
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
