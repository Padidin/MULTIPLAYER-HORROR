using cakeslice;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPlayer : MonoBehaviour
{
    public GameObject letakPisau;
    public GameObject letakPel;
    public Outline[] outlinePisau;
    public Outline[] outlinePel;
    public QuestCanvas[] quests;
    public GameObject timelineDialogQuest;



    public bool itemPertama = false;

    private void Awake()
    {
        quests = FindObjectsOfType<QuestCanvas>();
        timelineDialogQuest = GameObject.Find("Timeline Item Pertama");

        timelineDialogQuest.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pisau"))
        {
            letakPisau.SetActive(true);
            Destroy(other.gameObject);

            foreach(QuestCanvas canvasQuest in quests)
            {
                canvasQuest.AddProgress(1);
            }
        }
        if (other.CompareTag("Pel"))
        {
            letakPel.SetActive(true);
            Destroy(other.gameObject);

            foreach (QuestCanvas canvasQuest in quests)
            {
                canvasQuest.AddProgress(1);
            }
        }
    }

    public void GrabObject(string bendaApa)
    {
        if (bendaApa == "Pisau")
        {
            if (!itemPertama)
            {
                timelineDialogQuest.SetActive(true);

                itemPertama = true;
            }

            foreach (var objek in outlinePisau)
            {
                objek.eraseRenderer = false;
            }
        }else if (bendaApa == "Pel")
        {
            if (!itemPertama)
            {
                //Timeline dialog muncul quest mencari bukti

                itemPertama = true;
            }

            foreach (var objek in outlinePel)
            {
                objek.eraseRenderer = false;
            }
        }
    }

    public void DropObject(string bendaApa)
    {
        if (bendaApa == "Pisau")
        {
            foreach (var objek in outlinePisau)
            {
                objek.eraseRenderer = true;
            }
        }else if (bendaApa == "Pel")
        {
            foreach (var objek in outlinePel)
            {
                objek.eraseRenderer = true;
            }
        }
    }
}
