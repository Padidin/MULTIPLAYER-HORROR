using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MovingObjectMulti : MonoBehaviour, IInteractable
{
    [Header("Config")]
    public float maxBar;
    public float currentBar;
    public float beratBenda;
    public float kekuatanDorong;
    public cakeslice.Outline[] Outline;

    [Header("UI")]
    public GameObject canvasUi;
    public Image barUi;

    [Header("Anim / Cutscene")]
    public PlayableDirector geserBenda;

    private bool sudahTergeser = false;
    private float delayMenghilang = 3f;
    private float maxDelay = 3f;

    void Update()
    {
        if (sudahTergeser)
        {
            canvasUi.SetActive(false);
            HideOutline();
            return;
        }

        Mekanik();
        UpdateBarUI();
    }

    // ===============================
    //  IInteractable IMPLEMENTATION
    // ===============================

    public void Highlight(bool state)
    {
        if (sudahTergeser)
        {
            HideOutline();
            return;
        }

        foreach (var outline in Outline)
            outline.eraseRenderer = !state;
    }

    public void Interact(Playere player)
    {
        if (sudahTergeser) return;

        // tekan E → dorong objek
        currentBar += kekuatanDorong;
        canvasUi.SetActive(true);
        delayMenghilang = maxDelay;
    }

    // ===============================
    //  MEKANIK ORIGINAL
    // ===============================

    void Mekanik()
    {
        if (currentBar >= maxBar)
        {
            geserBenda.Play();
            sudahTergeser = true;
            return;
        }

        if (currentBar > 0 && currentBar <= maxBar)
            currentBar -= Time.deltaTime * beratBenda;

        if (currentBar < 0)
            currentBar = 0;
    }

    void UpdateBarUI()
    {
        barUi.fillAmount = currentBar / maxBar;

        if (currentBar == 0)
        {
            delayMenghilang -= Time.deltaTime;

            if (delayMenghilang <= 0)
            {
                canvasUi.SetActive(false);
                delayMenghilang = maxDelay;
            }
        }
        else
        {
            delayMenghilang = maxDelay;
        }
    }

    void HideOutline()
    {
        foreach (var outline in Outline)
            outline.eraseRenderer = true;
    }
}
