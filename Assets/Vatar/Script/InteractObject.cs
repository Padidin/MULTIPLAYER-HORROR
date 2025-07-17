using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public bool triggerBathup = false;
    public bool triggerRak = false;

    public bool canInteractBathup = false;
    public bool canInteractRak = false;

    private void Update()
    {
        if (triggerBathup && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(BathupInteract());
        }

        if (triggerRak && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(BathupInteract());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bathup"))
        {
            triggerBathup = true;
        }
        if (other.CompareTag("Rak"))
        {
            triggerRak = true;
        }
    }

    IEnumerator BathupInteract()
    {
        canInteractBathup = true;
        yield return new WaitForSeconds(5f);
        canInteractBathup = false;
    }
    IEnumerator RakInteract()
    {
        canInteractRak = true;
        yield return new WaitForSeconds(5f);
        canInteractRak = false;
    }
}
