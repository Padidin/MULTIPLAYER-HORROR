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

    // Update is called once per frame
    void Update()
    {
        if (terjangkau)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Nyala)
                {
                    cahayaLampu.SetActive(false);
                }
                else
                {
                    cahayaLampu.SetActive(true);
                }
                
            }
        }

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Argha") || other.CompareTag("Irul"))
        {
            terjangkau = true;

            foreach (Outline garisTepi in Outline)
            {
                garisTepi.eraseRenderer = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Argha") || other.CompareTag("Irul"))
        {
            terjangkau = false;

            foreach (Outline garisTepi in Outline)
            {
                garisTepi.eraseRenderer = true;
            }
        }
    }
}
