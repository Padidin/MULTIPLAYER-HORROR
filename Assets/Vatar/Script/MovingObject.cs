using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MovingObject : MonoBehaviour
{
    public float interactDistance;
    public float maxBar;
    public float currentBar;
    public float beratBenda;
    public float kekuatanDorong;

    public PlayableDirector geserBenda;
    public GameObject canvasUi;
    public Image barUi;
    public cakeslice.Outline[] Outline;


    private float delayMenghilang = 3f;
    private float maxDelay = 3f;
    private bool sudahTergeser = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sudahTergeser)
        {
            canvasUi.SetActive(false);
            foreach (cakeslice.Outline outline in Outline)
            {
                outline.eraseRenderer = true;
            }
        }
        if (sudahTergeser) return;
        Mekanik();
        barUiApdet();

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            MovingObject laci = hit.collider.GetComponent<MovingObject>();
            if (laci != null && laci == this)
            {
                foreach (cakeslice.Outline outline in Outline)
                {
                    outline.eraseRenderer = false;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentBar += kekuatanDorong;
                    canvasUi.SetActive(true);
                }
            }
            else
            {
                foreach (cakeslice.Outline outline in Outline)
                {
                    outline.eraseRenderer = true;
                }
            }
        }
        else
        {
            foreach (cakeslice.Outline outline in Outline)
            {
                outline.eraseRenderer = true;
            }
        }
    }

    void Mekanik()
    {
        if (currentBar >= maxBar)
        {
            geserBenda.Play();
            sudahTergeser = true;
        }

        if (currentBar > 0 && currentBar <= maxBar)
        {
            currentBar -= Time.deltaTime * beratBenda;
        }

        if (currentBar < 0)
        {
            currentBar = 0;
        }
    }

    void barUiApdet()
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
}
