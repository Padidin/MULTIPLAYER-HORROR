using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenObject : MonoBehaviour
{
    public float raycastDistance = 4f;
    public KeyCode interactButton = KeyCode.E;

    [SerializeField] private Image hotbarDefault;
    [SerializeField] private Color warnaAwal = Color.white;
    [SerializeField] private Color warnaRaycast = Color.yellow;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Animator animatorDoor;
    [SerializeField] private Animator animatorLaci;
    [SerializeField] private bool isOpenDoor;
    [SerializeField] private bool isOpenLaci;

    private void Update()
    {
        animatorDoor = null;
        animatorLaci = null;

        hotbarDefault.color = warnaAwal;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Door"))
            {
                animatorDoor = hit.collider.GetComponent<Animator>();
                hotbarDefault.color = warnaRaycast;

                if (Input.GetKeyDown(interactButton))
                {
                    if (animatorDoor != null)
                    {
                        isOpenDoor = !isOpenDoor;

                        animatorDoor.SetBool("IsOpen", isOpenDoor);
                    }
                }
            }

            if (hit.collider.CompareTag("Laci"))
            {
                animatorLaci = hit.collider.GetComponent<Animator>();
                hotbarDefault.color = warnaRaycast;

                if (Input.GetKeyDown(interactButton))
                {
                    if (animatorLaci != null)
                    {
                        isOpenLaci = !isOpenLaci;

                        animatorLaci.SetBool("IsOpen", isOpenLaci);
                    }
                }
            }
        }
    }
}
