using UnityEngine;

public class HeadLookAtPlayer : MonoBehaviour
{
    public Transform headBone;     // mixamorig:Head
    public Transform followTarget; // biasanya player HEAD / camera
    public float smooth = 6f;

    [Header("Clamp")]
    public float maxYaw = 60f;     // kiri-kanan
    public float maxPitch = 30f;   // atas-bawah

    private Quaternion defaultRot;

    void Start()
    {
        defaultRot = headBone.localRotation; // simpan rotasi awal
    }

    void LateUpdate()
    {
        if (headBone == null || followTarget == null) return;

        // arah target dalam LOCAL SPACE head parent
        Vector3 localDir = headBone.parent.InverseTransformPoint(followTarget.position);

        // dapatkan rotasi yang diinginkan
        Quaternion lookRot = Quaternion.LookRotation(localDir);

        // convert ke Euler biar bisa di-clamp
        Vector3 euler = lookRot.eulerAngles;

        // Benerin nilai jadi -180 sampai 180
        if (euler.y > 180) euler.y -= 360;
        if (euler.x > 180) euler.x -= 360;

        // clamp kanan-kiri (yaw)
        euler.y = Mathf.Clamp(euler.y, -maxYaw, maxYaw);

        // clamp atas-bawah (pitch)
        euler.x = Mathf.Clamp(euler.x, -maxPitch, maxPitch);

        // gabung lagi
        Quaternion clampedRot = Quaternion.Euler(euler);

        // apply smooth
        headBone.localRotation = Quaternion.Slerp(
            headBone.localRotation,
            defaultRot * clampedRot,
            Time.deltaTime * smooth
        );
    }
}
