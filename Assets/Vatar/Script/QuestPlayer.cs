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
    public GameObject timelineQuestAwal;

    public TextMeshProUGUI textQuest;
    public string textName;
    public int itemTerkumpul;
    public int maxItem = 6;

    public bool itemPertama = false;

    private void Awake()
    {
        quests = FindObjectsOfType<QuestCanvas>();
        timelineQuestAwal = GameObject.Find("Timeline Item Pertama");

        timelineQuestAwal.SetActive(false);

        itemTerkumpul = 0;

        UpdateUI();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pisau"))
        {
            letakPisau.SetActive(true);
            Destroy(other.gameObject);

            AddProgress(1);
        }
        if (other.CompareTag("Pel"))
        {
            letakPel.SetActive(true);
            Destroy(other.gameObject);

            AddProgress(1);
        }
    }

    public void GrabObject(string bendaApa)
    {
        if (bendaApa == "Pisau")
        {
            if (!itemPertama)
            {
                timelineQuestAwal.SetActive(true);

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

    public void AddProgress(int jumlah)
    {
        itemTerkumpul += jumlah;

        UpdateUI();
    }

    public void UpdateUI()
    {
        textQuest.text = $"{textName} ({itemTerkumpul}) ";

    }
}
