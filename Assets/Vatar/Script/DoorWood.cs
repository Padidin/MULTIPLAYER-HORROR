using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWood : MonoBehaviour
{
    public Animator animasi;
    public bool open;
    public bool terjangkau;
    public bool haveKey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (terjangkau)
        {
            if (haveKey)
            {
                if (open)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        open = false;
                        animasi.SetBool("open", false);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        open = true;
                        animasi.SetBool("open", true);
                    }
                }
            }
            else
            {
                // ini nanti ngeluarin efek suara pintu kekunci
            }
        }
    }

    private void OnTriggerEnter(Collider other)
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
    }
}
