using UnityEngine;

public class HeadRotateToPlayer : MonoBehaviour
{
    public Transform headBone;      // drag bone kepala (biasanya mixamorig:Head)
    public Transform player;        // drag player
    public float lookRadius = 8f;   // jarak maksimal buat nengok
    public float rotateSpeed = 5f;  // kecepatan rotasi kepala
    public float maxHeadTurn = 60f; // biar kepala gak muter kayak burung hantu 

    private Quaternion initialRotation;

    void Start()
    {
        if (headBone == null)
        {
            Debug.LogWarning("Belum assign headBone di inspector!");
            return;
        }

        initialRotation = headBone.localRotation;
    }

    void Update()
    {
        if (headBone == null || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= lookRadius)
        {
            Vector3 lookDir = player.position - headBone.position;
            lookDir.y = 0; // biar gak nunduk / mendongak berlebihan
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            Quaternion limitedRotation = LimitHeadRotation(targetRotation);

            headBone.rotation = Quaternion.Slerp(
                headBone.rotation,
                limitedRotation,
                Time.deltaTime * rotateSpeed
            );
        }
        else
        {
            // balik ke posisi awal
            headBone.localRotation = Quaternion.Slerp(
                headBone.localRotation,
                initialRotation,
                Time.deltaTime * rotateSpeed
            );
        }
    }

    Quaternion LimitHeadRotation(Quaternion target)
    {
        Quaternion localTarget = Quaternion.Inverse(transform.rotation) * target;
        Vector3 euler = localTarget.eulerAngles;

        if (euler.y > 180) euler.y -= 360;
        euler.y = Mathf.Clamp(euler.y, -maxHeadTurn, maxHeadTurn);

        return transform.rotation * Quaternion.Euler(0, euler.y, 0);
    }
}
