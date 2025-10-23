using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject cahayaLampu;
    public bool terjangkau;
    public bool Nyala;
    public Outline[] Outline;
    public AudioSource lightSwitch;
    public float interactDistance = 3f; 

    // Update is called once per frame
    void Update()
    {
        if (cahayaLampu != null)
        {
            if (cahayaLampu.activeInHierarchy)
            {
                Nyala = true;
            }
            else
            {
                Nyala = false;
            }
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            LightSwitch saklar = hit.collider.GetComponent<LightSwitch>();
            if (saklar != null && saklar == this)
            {
                foreach (Outline garisTepi in Outline)
                {
                    garisTepi.eraseRenderer = false;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (Nyala)
                    {
                        if (cahayaLampu != null)
                        {
                            cahayaLampu.SetActive(false);
                        }
                        lightSwitch.Play();
                    }
                    else
                    {
                        if (cahayaLampu != null)
                        {
                            cahayaLampu.SetActive(true);
                        }
                        lightSwitch.Play();
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
        else
        {
            foreach (Outline garisTepi in Outline)
            {
                garisTepi.eraseRenderer = true;
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Argha") || other.CompareTag("Irul"))
    //    {
    //        terjangkau = true;

    //        foreach (Outline garisTepi in Outline)
    //        {
    //            garisTepi.eraseRenderer = false;
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Argha") || other.CompareTag("Irul"))
    //    {
    //        terjangkau = false;

    //        foreach (Outline garisTepi in Outline)
    //        {
    //            garisTepi.eraseRenderer = true;
    //        }
    //    }
    //}
}
