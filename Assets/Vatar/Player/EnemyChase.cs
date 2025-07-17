using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyChase : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform player1;
    public Transform player2;
    public Camera playerCamera; 

    [Header("Enemy Settings")]
    public NavMeshAgent agent;
    public float detectionRange = 20f;
    public float patrolRadius = 10f;
    public float patrolDelay = 3f;

    [Header("UI Settings")]
    public GameObject caughtPanel;
    public Text caughtText;
    public Button backToMenuButton;
    public GameObject[] otherUIElements;

    private Transform target;
    private bool hasCaught = false;
    private float patrolTimer = 0f;
    private bool isPatrolling = false;

    void Start()
    {
        caughtPanel.SetActive(false);
        backToMenuButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (hasCaught) return;

        float distToP1 = Vector3.Distance(transform.position, player1.position);
        float distToP2 = Vector3.Distance(transform.position, player2.position);
        target = (distToP1 < distToP2) ? player1 : player2;

        if (Vector3.Distance(transform.position, target.position) <= detectionRange)
        {
            isPatrolling = false;
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
        else
        {
            StartPatrolling();
        }
    }

    void StartPatrolling()
    {
        if (!isPatrolling)
        {
            isPatrolling = true;
            patrolTimer = patrolDelay;
        }

        patrolTimer -= Time.deltaTime;
        if (patrolTimer <= 0f)
        {
            Vector3 randomDir = Random.insideUnitSphere * patrolRadius;
            randomDir += transform.position;

            if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                agent.isStopped = false;
            }

            patrolTimer = patrolDelay;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasCaught) return;

        if (other.transform == player1 || other.transform == player2)
        {
            hasCaught = true;
            agent.isStopped = true;

            foreach (GameObject ui in otherUIElements)
                ui.SetActive(false);

            if (playerCamera != null)
            {
                if (playerCamera.name.ToLower().Contains("1") && other.transform == player1)
                    caughtText.text = "Kamu tertangkap";
                else if (playerCamera.name.ToLower().Contains("2") && other.transform == player2)
                    caughtText.text = "Kamu tertangkap";
                else
                    caughtText.text = "Temanmu tertangkap";
            }
            else
            {
                caughtText.text = "Tertangkap";
            }

            caughtPanel.SetActive(true);
            Invoke(nameof(ShowButton), 2f);
        }
    }

    void ShowButton()
    {
        backToMenuButton.gameObject.SetActive(true);
    }
}
