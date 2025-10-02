using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenObject : MonoBehaviour
{
    public float raycastDistance = 4f;
    public KeyCode interactButton = KeyCode.E;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private Animator animatorDoor;
    [SerializeField] private bool isOpenDoor;

    private void Update()
    {
        animatorDoor = null;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Door"))
            {
                animatorDoor = hit.collider.GetComponent<Animator>();

                if (Input.GetKeyDown(interactButton))
                {
                    if (animatorDoor != null)
                    {
                        isOpenDoor = !isOpenDoor;

                        animatorDoor.SetBool("IsOpen", isOpenDoor);
                    }
                }
            }
        }
    }
}
