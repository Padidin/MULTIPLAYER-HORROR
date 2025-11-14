using UnityEngine;
using UnityEngine.Playables;

public class HeadLookIK : MonoBehaviour
{
    public float distance = 3f;
    public Animator anim;
    public Transform headBone;
    public Transform lookTarget;
    private bool canLook = false;

    public PlayableDirector director;

    void Start()
    {
        director.stopped += OnTimelineFinished;
    }

    void OnTimelineFinished(PlayableDirector d)
    {
        Debug.Log("Timeline selesai BRO!");
        canLook = true;
    }

    void LateUpdate()
    {
        if (!canLook) return;

        float dist = Vector3.Distance(transform.position, lookTarget.position);

        // Jika jarak masuk radius override kepala
        if (dist < distance)
        {
            LookAtPlayer();
        }
    }

    void LookAtPlayer()
    {
        Vector3 dir = lookTarget.position - headBone.position;
        Quaternion targetRot = Quaternion.LookRotation(dir);

        // Smooth rotasi biar gak robot
        headBone.rotation = Quaternion.Slerp(
            headBone.rotation,
            targetRot,
            Time.deltaTime * 5f
        );
    }
}
