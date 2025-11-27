using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using Photon.Pun;

public class MovingObjectMulti : MonoBehaviourPun, IInteractable
{
    [Header("Config")]
    public float maxBar;
    public float currentBar;
    public float beratBenda;
    public float kekuatanDorong;
    public cakeslice.Outline[] Outline;

    [Header("UI (Local Only)")]
    public GameObject canvasUi;
    public Image barUi;

    [Header("Anim / Cutscene")]
    public PlayableDirector geserBenda;

    private bool sudahTergeser = false;
    private float delayMenghilang = 3f;
    private float maxDelay = 3f;

    void Start()
    {
        // Pastikan kalau player join belakangan, state timeline ikut
        if (sudahTergeser)
        {
            geserBenda.time = geserBenda.duration;
            geserBenda.Evaluate();
        }
    }

    void Update()
    {
        if (sudahTergeser)
        {
            canvasUi.SetActive(false);
            HideOutline();
            return;
        }

        MekanikLocal();      // tetap lokal tapi akan sync via RPC
        UpdateBarUI();       // hanya local
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

        // local effect
        canvasUi.SetActive(true);
        delayMenghilang = maxDelay;

        // Tell all players to add dorongan
        photonView.RPC("RPC_AddDorongan", RpcTarget.All, kekuatanDorong);
    }

    // ===============================
    //     MULTIPLAYER RPC
    // ===============================

    [PunRPC]
    void RPC_AddDorongan(float amount)
    {
        currentBar += amount;

        // Check apakah sudah geser → notify semua client
        if (currentBar >= maxBar && !sudahTergeser)
        {
            photonView.RPC("RPC_PlayTimeline", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void RPC_PlayTimeline()
    {
        sudahTergeser = true;

        geserBenda.Play();        // timeline jalan ke semua client
        HideOutline();
        canvasUi.SetActive(false);
    }

    // ===============================
    // LOCAL MECHANIC ONLY
    // ===============================

    void MekanikLocal()
    {
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
