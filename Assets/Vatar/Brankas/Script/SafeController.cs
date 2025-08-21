using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SafeController : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;
    public Text codeDisplay;
    public Text warningText;
    public Text interactText;

    [Header("Safe Animation")]
    public Animator animator;

    private string currentInput = "";
    private string correctCode = "113091";
    private bool playerInRange = false;
    private bool isPanelOpen = false;
    private bool canInput = true;
    private bool isSafeOpened = false;

    void Start()
    {
        panel.SetActive(false);
        warningText.gameObject.SetActive(false);
        interactText.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (playerInRange && !isSafeOpened)
        {
            interactText.gameObject.SetActive(!isPanelOpen);
            if (Input.GetKeyDown(KeyCode.E) && !isPanelOpen)
            {
                OpenPanel();
            }
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }

    void OpenPanel()
    {
        panel.SetActive(true);
        isPanelOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        currentInput = "";
        UpdateDisplay();
    }

    public void PressNumber(string number)
    {
        if (!canInput || currentInput.Length >= 6) return;
        currentInput += number;
        UpdateDisplay();
    }

    public void PressDelete()
    {
        if (!canInput || currentInput.Length == 0) return;
        currentInput = currentInput.Substring(0, currentInput.Length - 1);
        UpdateDisplay();
    }

    public void PressConfirm()
    {
        if (!canInput) return;

        if (currentInput == correctCode)
        {
            animator.SetTrigger("OpenSafe");
            isSafeOpened = true;
            ClosePanel();
        }
        else
        {
            StartCoroutine(WrongCodeRoutine());
        }
    }

    IEnumerator WrongCodeRoutine()
    {
        canInput = false;
        currentInput = "";
        UpdateDisplay();

        warningText.text = "Wrong Code";
        warningText.color = Color.red;
        warningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        warningText.gameObject.SetActive(false);
        canInput = true;
    }

    void UpdateDisplay()
    {
        codeDisplay.text = currentInput;
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        isPanelOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isSafeOpened)
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (isPanelOpen) ClosePanel();
        }
    }
}
