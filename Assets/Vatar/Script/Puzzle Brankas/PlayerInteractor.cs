using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float interactDistance = 2f;
    public Playere playerMove;

    [Header("Assign Camera Player Secara Manual")]
    public Camera playerCamera;

    public LayerMask interactMask;

    void Update()
    {
        if (playerCamera == null) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactMask))
        {
            IInteractable interactObj = hit.collider.GetComponent<IInteractable>();

            if (interactObj != null)
            {
                interactObj.Highlight(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactObj.Interact(playerMove);
                }
            }
        }
    }
}
