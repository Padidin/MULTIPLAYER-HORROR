using Photon.Pun.Demo.Cockpit;
using UnityEngine;

public class RotateInspectMulti : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public bool bisaRotate;
    private bool isHolding = false;

    void Update()
    {
        Transform holder = GameObject.Find("Inspect Holder").transform;

        if (!transform.IsChildOf(holder)) return;
        //if (!ItemInspectManager.Instance.isInspecting) return;
        if (bisaRotate != true) return;
        if (Input.GetMouseButtonDown(0)) isHolding = true;
        if (Input.GetMouseButtonUp(0)) isHolding = false;

        if (isHolding)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.back, -mouseX, Space.World);
            transform.Rotate(Vector3.right, -mouseY, Space.World);
        }
    }
}
