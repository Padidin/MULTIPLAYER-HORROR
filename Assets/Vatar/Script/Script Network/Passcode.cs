using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Playables;

public class Passcode : MonoBehaviour, IInteractable
{
    [Header("UI")]
    public GameObject panel;
    public Text codeDisplay;
    public Text warningText;
    //public Text interactText; // "Press E to interact"
    public cakeslice.Outline[] outline;

    [Header("Safe Animation")]
    //public Animator animator;
    public PlayableDirector timelineRollingDoor;

    private string currentInput = "";
    private string correctCode = "113091";

    private bool isPanelOpen = false;
    private bool canInput = true;
    private bool isSafeOpened = false;
    private bool isFocused = false;

    void Start()
    {
        panel.SetActive(false);
        warningText.gameObject.SetActive(false);
        //interactText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isFocused || isSafeOpened)
        {
            //interactText.gameObject.SetActive(false);
            foreach (var outline in outline)
            {
                outline.eraseRenderer = true;
            }
            return;
        }

        // tampilkan tulisan "Press E"
        //interactText.gameObject.SetActive(!isPanelOpen);
    }

    // =========================================
    // IInteractable IMPLEMENTATION
    // =========================================
    public void Highlight(bool state)
    {
        isFocused = state;

        if (!state)
        {
            //interactText.gameObject.SetActive(false);
            HideOutline();
            if (isPanelOpen) ClosePanel();
        }

        foreach (var outline in outline)
        {
            outline.eraseRenderer = false;
        }
    }

    public void Interact(Playere player)
    {
        if (isSafeOpened) return;

        if (!isPanelOpen)
        {
            OpenPanel();
        }
    }

    // =========================================

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
            //animator.SetTrigger("OpenSafe");
            timelineRollingDoor.Play();
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

    void HideOutline()
    {
        foreach (var outline in outline)
            outline.eraseRenderer = true;
    }
}
